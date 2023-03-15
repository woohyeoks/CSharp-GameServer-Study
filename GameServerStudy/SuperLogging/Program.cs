using SuperLogging.Logging;
using SuperLogging.Logging.Factory;

namespace SuperLogging
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILogFactory factory = new ConsoleLogFactory();
            ILog log = factory.GetLog("testServer");
            log.Info("test");
            Console.WriteLine("Hello, World!");
        }
    }
}