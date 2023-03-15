using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLogging.Logging.Factory
{
    public interface ILogFactory
    {
        ILog GetLog(string name);
    }
}
