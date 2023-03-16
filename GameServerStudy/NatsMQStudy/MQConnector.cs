using NATS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatsMQStudy
{

    public struct MQConnInfo
    {
        private Dictionary<string, MQConnInfo> _MQReceivers;
        private Dictionary<string, MQConnInfo> _MQSenders;
        public IConnection conn;
        public IAsyncSubscription sub;
        public string subject;
        public string group;
        public Action<string, byte[]> action;
    }

    public class MQConnector
    {
        private string _mqServerAddr;
        private Options _mqOption;
        private string _subject;
        private string _qGroup;

        private Dictionary<string, MQConnInfo> _MQReceivers = new Dictionary<string, MQConnInfo>();
        private Dictionary<string, MQConnInfo> _MQSenders = new Dictionary<string, MQConnInfo>();

        public MQConnector(string mqServerAddr)
        {
            _mqServerAddr = mqServerAddr;
            _mqOption = ConnectionFactory.GetDefaultOptions();
            _mqOption.Url = _mqServerAddr;
            _MQReceivers = new Dictionary<string, MQConnInfo>();
            _MQSenders = new Dictionary<string, MQConnInfo>();
        }

        private IConnection MQConnect()
        {
            var mqConn = new ConnectionFactory().CreateConnection(_mqOption);
            return mqConn;
        }

        /// <summary>
        /// MQ서버에 Subcribe 하는 부분입니다.  Recv완료시  등록한 callback 함수가 자동으로 호출됩니다.
        /// </summary>
        public bool AddReceiverAndSubScribe(string subject, Action<string, byte[]> callback, string serverAddress = "", string qGroup = "")
        {
            string url = _mqServerAddr;
            if (string.IsNullOrEmpty(serverAddress) == false)
            {
                url = serverAddress;
                _mqOption.Url = url;
            }

            var recvConn = MQConnect();
            if (recvConn == null)
                return false;
            if (_MQReceivers.ContainsKey(subject))
                return false;

            var connInfo = new MQConnInfo();
            connInfo.conn = recvConn;
            connInfo.subject = subject;
            connInfo.group = qGroup;
            connInfo.action = callback;

            if (string.IsNullOrEmpty(qGroup))
            {
                connInfo.sub = connInfo.conn.SubscribeAsync(subject, OnSubscribeCompleted);
            }
            else
            {
                connInfo.sub = connInfo.conn.SubscribeAsync(subject, qGroup);
                connInfo.sub.MessageHandler += OnSubscribeCompleted;
                connInfo.sub.Start();
            }
            _MQReceivers.Add(subject, connInfo);
            return true;

        }


        public bool AddSender(string senderSubject, string serverAddress)
        {
            string url = _mqServerAddr;
            if (string.IsNullOrEmpty(serverAddress) == false)
            {
                url = serverAddress;
                _mqOption.Url = url;
            }

            var sendConn = MQConnect();
            if (sendConn == null)
                return false;
            if (_MQSenders.ContainsKey(serverAddress))
                return false;
            var connInfo = new MQConnInfo();
            connInfo.conn = sendConn;
            connInfo.subject = senderSubject;
            _MQSenders.Add(senderSubject, connInfo);
            return true;
        }


        private void OnSubscribeCompleted(object sender, MsgHandlerEventArgs args)
        {
            string subject = args.Message.Subject;
            byte[] data = args.Message.Data;

            if (_MQReceivers.ContainsKey(subject) == false)
            {
                return;
            }
            _MQReceivers[subject].action?.Invoke(subject, data);
        }


        public void Send(string key, string subject, byte[] sendData)
        {
            if (_MQSenders.ContainsKey(key) == false)
                return;
            _MQSenders[key].conn.Publish(subject, sendData);
        }



        public void DestroyReceiver(string subject)
        {
            if (_MQReceivers.ContainsKey(subject) == false)
            {
                return;
            }
            _MQReceivers[subject].sub?.Dispose();
            _MQReceivers[subject].conn?.Dispose();
            _MQReceivers.Remove(subject);
        }


        public void DestroySender(string subject)
        {
            if (_MQSenders.ContainsKey(subject) == false)
            {
                return;
            }
            _MQSenders[subject].sub?.Dispose();
            _MQSenders[subject].conn?.Dispose();
            _MQSenders.Remove(subject);
        }

        public void DestroyAll()
        {
            foreach (var info in _MQSenders.Values)
            {
                DestroySender(info.subject);
            }
            foreach (var info in _MQReceivers.Values)
            {
                DestroyReceiver(info.subject);
            }
            _MQSenders.Clear();
            _MQReceivers.Clear();
        }


    }
}
