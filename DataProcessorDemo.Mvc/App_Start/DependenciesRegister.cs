using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Mvc;
using DataProcessorDemo.Services;
using DataProcessorDemo.Services.Interfaces.IRepositories;
using DataProcessorDemo.Services.Interfaces;
using DataProcessorDemo.Sql.Repositories;
using DataProcessorDemo.Logging;
using DataProcessorDemo.Mvc.Helpers;
using DataProcessorDemo.Sql.Enums;

namespace DataProcessorDemo.Mvc.App_Start
{
    public static class DependenciesRegister
    {
        public static void RegisterAll()
        {
            var autofacBuilder = new ContainerBuilder();

            autofacBuilder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();
            autofacBuilder.RegisterAssemblyModules(typeof(MvcApplication).Assembly);
            autofacBuilder.RegisterModule<AutofacWebTypesModule>();

            autofacBuilder.RegisterType<BasicLogging>().As<ILogging>();
            autofacBuilder.RegisterType<UploadedFileRepository>().As<IUploadedFileRepository>();
            autofacBuilder.RegisterType<UploadedFileContentRepository>().As<IUploadedFileContentRepository>();
            autofacBuilder.RegisterType<CurrencyHelpers>().As<ICurrencyHelpers>();
            autofacBuilder.RegisterType<TaxDetailRepository>().As<ITaxDetailRepository>();
            autofacBuilder.RegisterType<UnprocessedDetailRepository>().As<IUnprocessedDetailRepository>();
            
            autofacBuilder.Register(c => new DataProcessingServices(
                c.Resolve<IUploadedFileRepository>(),
                c.Resolve<IUploadedFileContentRepository>(),
                c.Resolve<ICurrencyHelpers>(),
                c.Resolve<ITaxDetailRepository>(),
                c.Resolve<IUnprocessedDetailRepository>(),
                c.Resolve<ILogging>()))
            .As<IDataProcessingServices>();
            
            autofacBuilder.RegisterFilterProvider();

            var autofacContainer = autofacBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(autofacContainer));
        }
    }
}