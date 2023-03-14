using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SuperListener.Listener
{
    abstract class SocketListenerBase : ISocketListener
    {
        public ListenerInfo Info { get; private set; }
        public event NewClientAcceptHandler NewClientAccepted;
        public event ErrorHandler Error;
        public event EventHandler Stopped;

        public IPEndPoint EndPoint
        {
            get { return Info.EndPoint; }
        }

        protected SocketListenerBase(ListenerInfo info)
        {
            Info = info;
        }


        public abstract bool Start();
        public abstract void Stop();

        protected void OnError(Exception e)
        {
            var handler = Error;
            if (handler != null)
                handler(this, e);
        }

        protected void OnError(string errorMessage)
        {
            OnError(new Exception(errorMessage));
        }

        protected void OnStopped()
        {
            var handler = Stopped;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnNewClientAccepted(Socket socket, object state)
        {
            var handler = NewClientAccepted;

            if (handler != null)
                handler(this, socket, state);
        }

    }
}
