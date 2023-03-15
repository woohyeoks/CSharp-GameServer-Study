using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLogging.Logging.Factory
{
    public class ConsoleLogFactory : ILogFactory
    {
        public ILog GetLog(string name)
        {
            return new ConsoleLog(name);
        }
    }
}
