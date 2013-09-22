using System;
using System.Configuration;
using System.Reflection;
using Autofac;
using ENode.Autofac;
using ENode.Domain;
using ENode.Infrastructure;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;
using Configuration = NHibernate.Cfg.Configuration;

namespace Grandsys.Wfm.Services
{
    public class NHibernateInitializer
    {
        private string GetSqlConnection()
        {
            return ConfigurationManager.ConnectionStrings["SqlReadSideConnectionString"].ConnectionString;
        }

        public void Initialize(ContainerBuilder builder,params Assembly[] assemblies)
        {
            var config = new Configuration();
            config.SetProperty("show_sql", "true");
            config.SetProperty("format_sql", "true");

            config.SessionFactory()
                //.Proxy.Through<NHibernate.ByteCode.Castle.ProxyFactoryFactory>() // 注意不可去掉
                  .Integrate.Using<MsSql2005Dialect>()
               .Connected.By<SqlClientDriver>().Using(GetSqlConnection());

            foreach (var assembly in assemblies)
                config.AddAssembly(assembly);


            var update = new SchemaUpdate(config);
            update.Execute(false, true);         

            builder.RegisterInstance(config.BuildSessionFactory()).As<ISessionFactory>();
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("Grandsys Wfm Team Web Services", typeof(AppHost).Assembly, typeof(EvaluationItems).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            var builder = new ContainerBuilder();

            new NHibernateInitializer().Initialize(builder, typeof(ReadSide.Outsource.EvaluationItem).Assembly);

            new ENodeFrameworkSqlInitializer().Initialize(builder,
                //typeof(NoteSample.Domain.Note).Assembly,
                typeof(Backend.Outsource.Events.EvaluationItemCreated).Assembly,
                typeof(Backend.Outsource.Domain.EvaluationItem).Assembly,
                typeof(ReadSide.Outsource.EvaluationItem).Assembly);


            ObjectContainer.Resolve<IMemoryCacheRebuilder>().RebuildMemoryCache();
            

            container.Adapter = new AutofacIocAdapter(((AutofacObjectContainer)ObjectContainer.Current).Container);

            //Routes
            //    .Add<Note>("/hello")
            //    .Add<Note>("/hello/{Name}", "POST")
            //    .Add<Note>("/hello/{Id}", "GET");
        }
    }
}