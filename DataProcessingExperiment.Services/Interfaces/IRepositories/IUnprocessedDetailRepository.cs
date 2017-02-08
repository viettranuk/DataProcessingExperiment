using System.Collections.Generic;
using System.Threading.Tasks;
using DataProcessingExperiment.Models.DTOs;

namespace DataProcessingExperiment.Services.Interfaces.IRepositories
{
    public interface IUnprocessedDetailRepository
    {
        Task AddUnprocessedDetailAsync(string insertUnprocessedDetailSql);
        List<UnprocessedDetailDto> GetUnprocessedDetailsByFileId(int fileId);
    }
}
