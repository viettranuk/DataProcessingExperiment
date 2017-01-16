using DataProcessingExperiment.Models.DTOs;

namespace DataProcessingExperiment.Sql
{
    public partial class UploadedFile
    {
        internal UploadedFileDto ToUploadedFileDto()
        {
            return new UploadedFileDto
            {
                FileId = this.FileId,
                Name = this.Name,
                Description = this.Description,
                Size = this.Size,
                ImportedOn = this.ImportedOn
            };
        }
    }
}
