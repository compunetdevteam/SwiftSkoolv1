using Microsoft.Practices.Unity;
using SwiftSkoolv1.WebUI.APIRepository.RepoAbstractions;
using SwiftSkoolv1.WebUI.APIRepository.Repository;
using SwiftSkoolv1.WebUI.Models;
using System.Data.Entity;
using System.Web.Http;
using Unity.WebApi;

namespace SwiftSkoolv1.WebUI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //CompunetDevTeam register and resolve code
            container.RegisterType<DbContext, SwiftSkoolDbContext>(new PerThreadLifetimeManager());
            container.RegisterType<IStudentRepository, StudentRepository>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}