using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Autofac;
using ENode.Autofac;
using ENode.JsonNet;
using ENode.Log4Net;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;

namespace Grandsys.Wfm.Services
{
    public class AutofacIocAdapter : IContainerAdapter
    {
        private readonly IContainer _container;

        public AutofacIocAdapter(IContainer container = null)
        {
            _container = container ?? new Autofac.ContainerBuilder().Build();
        }

        public T Resolve<T>()
        {

            return _container.Resolve<T>();
        }

        public T TryResolve<T>()
        {
            T result;

            if (_container.TryResolve<T>(out result))
            {
                return result;
            }

            return default(T);
        }
    }

    public class ENodeFrameworkSqlInitializer
    {

        private string GetSqlConnection()
        {
            return ConfigurationManager.ConnectionStrings["SqlEventSourcingConnectionString"].ConnectionString;
        }

        public void Initialize(ContainerBuilder builder, params Assembly[] assemblies)
        {
            ENode.Configuration
                .Create()
                .UseAutofac(builder)
                .RegisterFrameworkComponents()
                .RegisterBusinessComponents(assemblies)
                .UseLog4Net()
                .UseJsonNet()
                .UseSql(GetSqlConnection())
                .CreateAllDefaultProcessors()
                .Initialize(assemblies)
                .Start();
        }
    }

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}