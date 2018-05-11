using Footieworld.core.Services;
using Footieworld.core.Services.Interfaces;
using FootieWorld.data.ef;
using FootieWorld.data.ef.UnitOfWork;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace FootieWorldApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            container.RegisterType<IUnitOfWork, UnitOfWork>( new InjectionConstructor(new FootieDbEntities()));
            container.RegisterType<IStadiumService, StadiumService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}