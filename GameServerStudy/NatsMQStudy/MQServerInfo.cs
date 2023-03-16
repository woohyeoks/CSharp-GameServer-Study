using NATS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatsMQStudy
{
    public enum eServerType
    {
        SERVER_TYPE_NONE = 0,
        SERVER_TYPE_CENTER,
        SERVER_TYPE_DB,
        SERVER_TYPE_CAHT,
        SERVER_TYPE_GATE,
    }

    public static  class MQServerInfo
    {
        public const string MQGateServerSubjectPrefix = "gate";
        public const string MQGameServerSubjectPrefix = "game";
        public const string MQLobbyServerSubjectPrefix = "lobby";
        public const string MQMatchServerSubjectPrefix = "match";
        public const string MQDBServerSubject = "db";
        public const string MQChatServerSubjectPrefix = "CHAT";

        public static string MakeSubject(eServerType start, eServerType dest, int serverIndex = -1)
        {
            string subject = "";
            if (serverIndex > 0)
            {
                subject = ConvertSubName(start) + ".to." + ConvertSubName(dest) + serverIndex;
            }
            else
            {
                subject = ConvertSubName(start) + ".to." + ConvertSubName(dest);
            }
            return subject;
        }

        public static string ConvertSubName(eServerType type)
        {
            string subName = "";
            switch (type)
            {
                case eServerType.SERVER_TYPE_GATE:
                    subName = MQGateServerSubjectPrefix;
                    break;
                case eServerType.SERVER_TYPE_DB:
                    subName = MQDBServerSubject;
                    break;
            }
            return subName;
        }
    }

  

    public struct MQConnInfo
    {
        private Dictionary<string, MQConnInfo> _MQReceivers = new Dictionary<string, MQConnInfo>();
        private Dictionary<string, MQConnInfo> _MQSenders = new Dictionary<string, MQConnInfo>();
        public IConnection conn;
        public IAsyncSubscription sub;
        public string subject;
        public string group;
        public Action<string, byte[]> action;
    }


}
