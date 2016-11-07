using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataProcessorDemo.Mvc.Helpers;

namespace DataProcessorDemo.Mvc.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogAndHandleErrorAttribute());
        }
    }
}