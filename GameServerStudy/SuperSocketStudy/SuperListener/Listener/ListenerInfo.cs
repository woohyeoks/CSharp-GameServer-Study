using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SuperListener.Listener
{
    [Serializable]
    public class ListenerInfo
    {
        public IPEndPoint EndPoint { get; set; }
        public int BackLog { get; set; }
        public SslProtocols Security { get; set; }
    }
}
