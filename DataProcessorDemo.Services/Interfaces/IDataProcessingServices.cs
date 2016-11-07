using System.Threading.Tasks;
using System.Web;
using DataProcessorDemo.Models.ViewModels;

namespace DataProcessorDemo.Services.Interfaces
{
    public interface IDataProcessingServices
    {
        Task AddFileAsync(HttpPostedFileBase file);
        UploadHistoryViewModel GetUploadHistoryViewModel();
        TransactionLogViewModel GetTransactionLogViewModel(int fileId);
    }
}
