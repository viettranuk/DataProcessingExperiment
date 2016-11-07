using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessorDemo.Services.Interfaces;

namespace DataProcessorDemo.Logging
{
    public class BasicLogging : ILogging
    {
        public void LogException(Exception ex)
        {
            string filePath = @"C:\ExceptionLog.txt";

            using (var writer = new StreamWriter(filePath, true))
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