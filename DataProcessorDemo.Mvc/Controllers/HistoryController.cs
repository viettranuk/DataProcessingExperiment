using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataProcessorDemo.Services.Interfaces;

namespace DataProcessorDemo.Mvc.Controllers
{
    public class HistoryController : BaseController
    {
        protected readonly IDataProcessingServices _dataProcessingServices;

        public HistoryController(IDataProcessingServices dataProcessingServices)
        {
            if (dataProcessingServices == null)
            {
                throw new ArgumentNullException("dataProcessingServices");
            }

            this._dataProcessingServices = dataProcessingServices;
        }
        
        public ActionResult Index()
        {
            return View(_dataProcessingServices.GetUploadHistoryViewModel());
        }

        public ActionResult ViewTransactionLog(int fileId)
        {
            return View(_dataProcessingServices.GetTransactionLogViewModel(fileId));
        }
    }
}