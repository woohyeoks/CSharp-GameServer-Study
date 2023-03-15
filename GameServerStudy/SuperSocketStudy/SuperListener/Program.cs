using SuperListener.Listener;
using System.Net;

namespace SuperListener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ListenerInfo info = new ListenerInfo();

            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            info.EndPoint = new IPEndPoint(ipAddr, 8888);
            info.BackLog = 100;

            var listener = new TcpAsyncSocketListener(info);
            listener.Start();

            while (true)
            {

            }


        }
        

    }
}