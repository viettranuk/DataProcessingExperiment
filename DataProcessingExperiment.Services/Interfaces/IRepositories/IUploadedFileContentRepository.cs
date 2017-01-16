using System.IO;
using System.Threading.Tasks;

namespace DataProcessingExperiment.Services.Interfaces.IRepositories
{
    public interface IUploadedFileContentRepository
    {
        Task AddFileDataAsync(int fileId, byte[] inputStream);
    }
}
