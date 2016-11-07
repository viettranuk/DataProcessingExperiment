using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessorDemo.Services.Interfaces
{
    public interface ILogging
    {
        void LogException(Exception ex);
    }
}
