using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLogging.Logging.Factory
{
    public abstract class LogFactoryBase : ILogFactory
    {
        public abstract ILog GetLog(string name);
    }
}
