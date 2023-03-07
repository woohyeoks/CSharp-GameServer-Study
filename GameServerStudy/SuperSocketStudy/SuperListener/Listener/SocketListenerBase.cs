using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SuperListener.Listener
{
    abstract class SocketListenerBase : ISocketListener
    {
        public ListenerInfo Info { get; private set; }

        public IPEndPoint EndPoint
        {
            get { return Info.EndPoint; }
        }

        protected SocketListenerBase(ListenerInfo info)
        {
            Info = info;
        }


        public abstract bool Start();




    }
}
