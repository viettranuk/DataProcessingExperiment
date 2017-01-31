using System.Collections.Generic;
using System.Threading.Tasks;
using DataProcessingExperiment.Models.DTOs;

namespace DataProcessingExperiment.Services.Interfaces.IRepositories
{
    public interface IUnprocessedDetailRepository
    {
        void AddUnprocessedDetail(string insertUnprocessedDetailSql);
        List<UnprocessedDetailDto> GetUnprocessedDetailsByFileId(int fileId);
    }
}
