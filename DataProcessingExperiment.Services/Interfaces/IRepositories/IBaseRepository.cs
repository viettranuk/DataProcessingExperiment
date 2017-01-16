﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingExperiment.Services.Interfaces.IRepositories
{
    public interface IBaseRepository : IBaseReadOnlyRepository, 
        ITaxDetailRepository, 
        IUnprocessedDetailRepository,
        IUploadedFileContentRepository, 
        IUploadedFileRepository
    {
        void Create<TEntity>(TEntity entity, string createdBy = null) where TEntity : class;
        void Update<TEntity>(TEntity entity, string modifiedBy = null) where TEntity : class;
        void Delete<TEntity>(object id) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Save();
        Task SaveAsync();
    }
}
