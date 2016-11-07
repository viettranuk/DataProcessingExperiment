using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataProcessorDemo.Services.Interfaces;

namespace DataProcessorDemo.Mvc.Helpers
{
    public class LogAndHandleErrorAttribute : HandleErrorAttribute
    {
        private ILogging Logger { get; set; }
        
        public override void OnException(ExceptionContext filterContext)
        {
            Logger.LogException(filterContext.Exception);

            base.OnException(filterContext);
        }
    }
}