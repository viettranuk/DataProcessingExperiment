using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Mvc;
using DataProcessingExperiment.Services;
using DataProcessingExperiment.Services.Interfaces.IRepositories;
using DataProcessingExperiment.Services.Interfaces;
using DataProcessingExperiment.Sql.Repositories;
using DataProcessingExperiment.Logging;
using DataProcessingExperiment.Mvc.Helpers;
using DataProcessingExperiment.Sql.Enums;
using DataProcessingExperiment.Sql;

namespace DataProcessingExperiment.Mvc.App_Start
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
            autofacBuilder.RegisterType<CurrencyHelpers>().As<ICurrencyHelpers>();

            // Try InstancePerLifetimeScope() here to see if performance gets better??
            autofacBuilder.RegisterType<TaxEntities>().AsSelf().InstancePerRequest();
            
            autofacBuilder.RegisterType<BaseRepository<TaxEntities>>().As<IBaseRepository>();
            autofacBuilder.RegisterType<DataProcessingServices>().As<IDataProcessingServices>();
            
            autofacBuilder.RegisterFilterProvider();

            var autofacContainer = autofacBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(autofacContainer));
        }
    }
}