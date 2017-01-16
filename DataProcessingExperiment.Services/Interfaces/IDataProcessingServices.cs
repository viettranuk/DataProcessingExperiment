using System.Threading.Tasks;
using System.Web;
using DataProcessingExperiment.Models.ViewModels;

namespace DataProcessingExperiment.Services.Interfaces
{
    public interface IDataProcessingServices
    {
        Task AddFileAsync(HttpPostedFileBase file);
        UploadHistoryViewModel GetUploadHistoryViewModel();
        TransactionLogViewModel GetTransactionLogViewModel(int fileId);
    }
}
