using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SuperListener.Listener
{
    delegate void ErrorHandler(ISocketListener listener, Exception e);
    delegate void NewClientAcceptHandler(ISocketListener listener, Socket client, object state);

    interface ISocketListener
    {
        ListenerInfo Info { get; }
        IPEndPoint EndPoint { get; }
        bool Start();
        void Stop();


        event NewClientAcceptHandler NewClientAccepted;
        event ErrorHandler Error;
        event EventHandler Stopped;
    }
}
