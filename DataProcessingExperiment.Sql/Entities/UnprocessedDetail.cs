using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessingExperiment.Models.DTOs;

namespace DataProcessingExperiment.Sql
{
    public partial class UnprocessedDetail
    {
        internal UnprocessedDetailDto ToUnprocessedDetailDto()
        {
            return new UnprocessedDetailDto
            {
                FileId = this.FileId,
                LineData = this.LineData
            };
        }
    }
}
