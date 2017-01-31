using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DataProcessingExperiment.Models.DTOs;
using DataProcessingExperiment.Models.ViewModels;
using DataProcessingExperiment.Services.Interfaces;
using DataProcessingExperiment.Services.Interfaces.IRepositories;

namespace DataProcessingExperiment.Services
{
    public class DataProcessingServices : IDataProcessingServices
    {
        private readonly IBaseRepository _baseRepo;
        private readonly ICurrencyHelpers _currencyCodeValidator;
        private readonly ILogging _logger;

        public DataProcessingServices(IBaseRepository baseRepo, ICurrencyHelpers currencyCodeValidator, ILogging logger)
        {
            if (baseRepo == null)
            {
                throw new ArgumentNullException("baseRepo");
            }

            this._baseRepo = baseRepo;

            if (currencyCodeValidator == null)
            {
                throw new ArgumentNullException("currencyCodeValidator");
            }

            this._currencyCodeValidator = currencyCodeValidator;

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

                var fileId = await _baseRepo.AddFileDetailsAsync(fileRecord);

                return fileId;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return -1;
            }            
        }

        private int ProcessFileData(int fileId, byte[] inputStream)
        {
            try
            {
                var watch = Stopwatch.StartNew();                
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
                            _baseRepo.AddTaxDetail(processedValues.ToString().TrimEnd(','));

                            processedValues.Clear();
                        }

                        if ((unProcessedCount > 0) && (unProcessedCount % 100 == 0))
                        {
                            _baseRepo.AddUnprocessedDetail(unProcessedValues.ToString().TrimEnd(','));

                            unProcessedValues.Clear();
                        }                        
                    }
                                        
                    if (processedValues.Length > 0)
                    {
                        _baseRepo.AddTaxDetail(processedValues.ToString().TrimEnd(','));
                    }

                    if (unProcessedValues.Length > 0)
                    {
                        _baseRepo.AddUnprocessedDetail(unProcessedValues.ToString().TrimEnd(','));
                    }
                }

                watch.Stop();
                
                _logger.LogProcessingTime("ProcessFileDataAsync", watch.ElapsedMilliseconds, null);

                return processedCount;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
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
                    
                    await _baseRepo.AddFileDataAsync(fileId, payload);
                    
                    var processFileDataTask = ProcessFileData(fileId, payload);
                    
                    await _baseRepo.UpdateProcessedLineCountAsync(fileId, processFileDataTask);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
            }
        }

        public UploadHistoryViewModel GetUploadHistoryViewModel()
        {
            try
            {
                var uploadHistoryViewModel = new UploadHistoryViewModel
                {
                    UploadHistories = _baseRepo.GetAllUploadedFiles()
                };

                return uploadHistoryViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return null;
            }
        }

        public TransactionLogViewModel GetTransactionLogViewModel(int fileId)
        {
            try
            {
                var viewModel = new TransactionLogViewModel
                {
                    FileName = _baseRepo.GetFileNameById(fileId),
                    ProcessedLineCount = _baseRepo.GetProcessedLineCountForFile(fileId),
                    ProcessedTaxDetails = _baseRepo.GetProcessedTaxDetailsByFileId(fileId),
                    UnprocessedDetails = _baseRepo.GetUnprocessedDetailsByFileId(fileId)
                };

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, null);
                return null;
            }
        }
    }
}
