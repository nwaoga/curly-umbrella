using Footieworld.core.Services;
using Footieworld.core.Services.Interfaces;
using FootieWorld.data.ef;
using FootieWorld.data.ef.UnitOfWork;
using FootieWorldApi;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace FootieWorldApi
 {
     public static class NinjectWebCommon
{
    private static readonly Bootstrapper bootstrapper = new Bootstrapper();

    public static void Start()
    {
        DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
        DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        bootstrapper.Initialize(CreateKernel);
    }

    public static void Stop()
    {
        bootstrapper.ShutDown();
    }

    private static IKernel CreateKernel()
    {
        var kernel = new StandardKernel();
        kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
        kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

        RegisterServices(kernel);
        return kernel;
    }
    private static void RegisterServices(IKernel kernel)
    {
            //kernel.Bind<IRepo>().ToMethod(ctx => new Repo("Ninject Rocks!"));            
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope().WithConstructorArgument(new FootieDbEntities());
            kernel.Bind<DbContext>().To<FootieDbEntities>().InRequestScope();
            kernel.Bind<IStadiumService>().To<StadiumService>();

    }
}
}