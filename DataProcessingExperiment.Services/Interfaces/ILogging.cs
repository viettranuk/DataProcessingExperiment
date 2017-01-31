using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingExperiment.Services.Interfaces
{
    public interface ILogging
    {
        void LogException(Exception ex, string logFilePath);
        void LogProcessingTime(string methodName, long timeTakenInMilliseconds, string logFilePath);
    }
}
