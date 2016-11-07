using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataProcessorDemo.Models.ViewModels;
using DataProcessorDemo.Services.Interfaces;

namespace DataProcessorDemo.Mvc.Controllers
{
    public class HomeController : BaseController
    {
        protected readonly IDataProcessingServices _dataProcessingServices;

        public HomeController(IDataProcessingServices dataProcessingServices)
        {
            if (dataProcessingServices == null)
            {
                throw new ArgumentNullException("dataProcessingServices");
            }

            this._dataProcessingServices = dataProcessingServices;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.File != null)
                {
                    await _dataProcessingServices.AddFileAsync(model.File);
                }
            }

            return View("Index");
        }
    }
}