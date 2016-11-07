using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DataProcessorDemo.Models.DTOs;
using DataProcessorDemo.Models.ViewModels;
using DataProcessorDemo.Services.Interfaces;
using DataProcessorDemo.Services.Interfaces.IRepositories;

namespace DataProcessorDemo.Services
{
    public class DataProcessingServices : IDataProcessingServices
    {
        private readonly IUploadedFileRepository _uploadedFileRepo;
        private readonly IUploadedFileContentRepository _uploadedFileContentRepo;
        private readonly ICurrencyHelpers _currencyCodeValidator;
        private readonly ITaxDetailRepository _taxDetailRepo;
        private readonly IUnprocessedDetailRepository _unprocessedDetailRepo;
        private readonly ILogging _logger;

        public DataProcessingServices(
            IUploadedFileRepository uploadedFileRepo, 
            IUploadedFileContentRepository uploadedFileContentRepo,
            ICurrencyHelpers currencyCodeValidator,
            ITaxDetailRepository taxDetailRepo,
            IUnprocessedDetailRepository unprocessedDetailRepo,
            ILogging logger)
        {
            if (uploadedFileRepo == null)
            {
                throw new ArgumentNullException("uploadedFileRepo");
            }

            this._uploadedFileRepo = uploadedFileRepo;

            if (uploadedFileContentRepo == null)
            {
                throw new ArgumentNullException("uploadedFileContentRepo");
            }

            this._uploadedFileContentRepo = uploadedFileContentRepo;

            if (currencyCodeValidator == null)
            {
                throw new ArgumentNullException("currencyCodeValidator");
            }

            this._currencyCodeValidator = currencyCodeValidator;

            if (taxDetailRepo == null)
            {
                throw new ArgumentNullException("taxDetailRepo");
            }

            this._taxDetailRepo = taxDetailRepo;

            if (unprocessedDetailRepo == null)
            {
                throw new ArgumentNullException("unprocessedDetailRepo");
            }

            this._unprocessedDetailRepo = unprocessedDetailRepo;

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this._logger = logger;
        }

        private async Task<int> AddFileDetailsAsync(string name, string description, long size)
        {
            try
            {
                var fileRecord = new UploadedFileDto
                {
                    Name = name,
                    Description = description,
                    Size = size
                };

                var fileId = await _uploadedFileRepo.AddFileDetailsAsync(fileRecord);

                return fileId;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return -1;
            }            
        }

        private async Task<int> ProcessFileDataAsync(int fileId, byte[] inputStream)
        {
            try
            {
                var processedCount = 0;

                using (var streamReader = new StreamReader(new MemoryStream(inputStream)))
                {
                    var headerLine = streamReader.ReadLine();                    
                    var processedValues = new StringBuilder();
                    var unProcessedValues = new StringBuilder();
                    var parsedCount = 1;
                    var unProcessedCount = 0;

                    string currentLine;
                    while ((currentLine = streamReader.ReadLine()) != null)
                    {
                        var lineItems = currentLine.Split('|');

                        if (lineItems.Count() != 4)
                        {
                            unProcessedValues.Append("('");
                            unProcessedValues.Append(fileId.ToString());
                            unProcessedValues.Append("', '");
                            unProcessedValues.Append(currentLine);
                            unProcessedValues.Append("'),");

                            unProcessedCount++;
                        }
                        else
                        {
                            decimal amount;
                            var validAmount = decimal.TryParse(lineItems[3], out amount);

                            if (!string.IsNullOrWhiteSpace(lineItems[0]) &&
                                !string.IsNullOrWhiteSpace(lineItems[1]) &&
                                _currencyCodeValidator.IsValidCurrencyCode(lineItems[2]) &&
                                validAmount)
                            {                                
                                processedValues.Append("('");
                                processedValues.Append(fileId.ToString());
                                processedValues.Append("', '");
                                processedValues.Append(lineItems[0]); // Account
                                processedValues.Append("', '");
                                processedValues.Append(lineItems[1]); // Description
                                processedValues.Append("', '");
                                processedValues.Append(_currencyCodeValidator.ToCurrencyCodeEnumValue(lineItems[2]));
                                processedValues.Append("', '");
                                processedValues.Append(amount);
                                processedValues.Append("'),");

                                processedCount++;
                            }
                            else
                            {
                                unProcessedValues.Append("('");
                                unProcessedValues.Append(fileId.ToString());
                                unProcessedValues.Append("', '");
                                unProcessedValues.Append(currentLine);
                                unProcessedValues.Append("'),");

                                unProcessedCount++;
                            }
                        }

                        parsedCount++;

                        if ((processedCount > 0) && (processedCount % 100 == 0))
                        {
                            await _taxDetailRepo.AddTaxDetailAsync(processedValues.ToString().TrimEnd(','));

                            processedValues.Clear();
                        }

                        if ((unProcessedCount > 0) && (unProcessedCount % 100 == 0))
                        {
                            await _unprocessedDetailRepo.AddUnprocessedDetailAsync(unProcessedValues.ToString().TrimEnd(','));

                            unProcessedValues.Clear();
                        }                        
                    }
                                        
                    if (processedValues.Length > 0)
                    {
                        await _taxDetailRepo.AddTaxDetailAsync(processedValues.ToString().TrimEnd(','));
                    }

                    if (unProcessedValues.Length > 0)
                    {
                        await _unprocessedDetailRepo.AddUnprocessedDetailAsync(unProcessedValues.ToString().TrimEnd(','));
                    }
                }

                return processedCount;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return 0;
            }
        }

        public async Task AddFileAsync(HttpPostedFileBase file)
        {
            try
            {
                var filename = Path.GetFileName(file.FileName);
                var fileId = await AddFileDetailsAsync(filename, file.FileName, file.ContentLength);
                
                using (var memStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memStream);

                    var payload = memStream.ToArray();
                    var addFileDataTask = _uploadedFileContentRepo.AddFileDataAsync(fileId, payload);
                    var processFileDataTask = ProcessFileDataAsync(fileId, payload);
                    
                    await Task.WhenAll(addFileDataTask, processFileDataTask);
                    await _uploadedFileRepo.UpdateProcessedLineCount(fileId, processFileDataTask.Result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        public UploadHistoryViewModel GetUploadHistoryViewModel()
        {
            try
            {
                var uploadHistoryViewModel = new UploadHistoryViewModel
                {
                    UploadHistories = _uploadedFileRepo.GetAllUploadedFiles()
                };

                return uploadHistoryViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return null;
            }
        }

        public TransactionLogViewModel GetTransactionLogViewModel(int fileId)
        {
            try
            {
                var viewModel = new TransactionLogViewModel
                {
                    FileName = _uploadedFileRepo.GetFileNameById(fileId),
                    ProcessedLineCount = _uploadedFileRepo.GetProcessedLineCountForFile(fileId),
                    ProcessedTaxDetails = _taxDetailRepo.GetProcessedTaxDetailsByFileId(fileId),
                    UnprocessedDetails = _unprocessedDetailRepo.GetUnprocessedDetailsByFileId(fileId)
                };

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return null;
            }
        }
    }
}
