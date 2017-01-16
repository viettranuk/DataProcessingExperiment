using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessingExperiment.Services.Interfaces;

namespace DataProcessingExperiment.Logging
{
    public class BasicLogging : ILogging
    {
        public void LogException(Exception ex, string logFilePath)
        {
            // TODO: replace by Log4Net
            
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DefaultExceptionLog.txt");
            }

            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(
                    "Message : " + ex.Message +
                    Environment.NewLine +
                    "InnerException : " + ex.InnerException +
                    Environment.NewLine +
                    "StackTrace : " + ex.StackTrace +
                    Environment.NewLine +
                    "Date : " + DateTime.Now.ToString()
                );

                writer.WriteLine(
                    Environment.NewLine +
                    "-----------------------------------------------------------------------------" +
                    Environment.NewLine
                );
            }
        }
    }
}