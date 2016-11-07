using System.IO;
using System.Threading.Tasks;

namespace DataProcessorDemo.Services.Interfaces.IRepositories
{
    public interface IUploadedFileContentRepository
    {
        Task AddFileDataAsync(int fileId, byte[] inputStream);
    }
}
