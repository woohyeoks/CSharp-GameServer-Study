using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SuperListener.Listener
{
    class TcpAsyncSocketListener : SocketListenerBase
    {
        private int m_ListenBackLog;
        private Socket m_ListenSocket;
        private SocketAsyncEventArgs m_AcceptSAE;

        public TcpAsyncSocketListener(ListenerInfo info)
            : base(info)
        {
            m_ListenBackLog = info.BackLog;
        }

        public override bool Start()
        {
            m_ListenSocket = new Socket(this.Info.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                m_ListenSocket.Bind(this.Info.EndPoint);
                m_ListenSocket.Listen(this.Info.BackLog);

                //아래 옵션은 사용하지 않는다. 만약 사용한다면 아래 API는 닷넷코어에서 지원하지 않으므로 바꾸어야 한다.
                //m_ListenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                //m_ListenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

                SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(acceptEventArg_Completed);
                m_AcceptSAE = acceptEventArg;

                if (!m_ListenSocket.AcceptAsync(acceptEventArg))
                    ProcessAccept(acceptEventArg);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private void acceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Socket socket = null;
            if (e.SocketError != SocketError.Success)
            {
                var errorCode = (int)e.SocketError;

                //The listen socket was closed
                if (errorCode == 995 || errorCode == 10004 || errorCode == 10038)
                    return;
            }
            else
            {
                socket = e.AcceptSocket;
            }

            e.AcceptSocket = null;
            bool willRaiseEvent = false;

            try
            {
                willRaiseEvent = m_ListenSocket.AcceptAsync(e);
            }
            catch (Exception exc)
            {
                willRaiseEvent = true;
            }

            if (!willRaiseEvent)
                ProcessAccept(e);
        }
    }
}
