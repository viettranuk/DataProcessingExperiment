using System.Collections.Generic;
using System.Threading.Tasks;
using DataProcessorDemo.Models.DTOs;

namespace DataProcessorDemo.Services.Interfaces.IRepositories
{
    public interface IUnprocessedDetailRepository
    {
        Task AddUnprocessedDetailAsync(string insertUnprocessedDetailSql);
        List<UnprocessedDetailDto> GetUnprocessedDetailsByFileId(int fileId);
    }
}
