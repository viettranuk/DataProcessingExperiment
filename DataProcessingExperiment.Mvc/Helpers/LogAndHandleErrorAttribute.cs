using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataProcessingExperiment.Services.Interfaces;

namespace DataProcessingExperiment.Mvc.Helpers
{
    public class LogAndHandleErrorAttribute : HandleErrorAttribute
    {
        private ILogging Logger { get; set; }
        
        public override void OnException(ExceptionContext filterContext)
        {
            Logger.LogException(filterContext.Exception, null);

            base.OnException(filterContext);
        }
    }
}