using System.Web;

namespace DataProcessingExperiment.Models.ViewModels
{
    public class UploadViewModel
    {
        public HttpPostedFileBase File { get; set; }
        public string FileName { get; set; }
    }
}