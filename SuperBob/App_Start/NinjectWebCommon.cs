[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SuperBob.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SuperBob.App_Start.NinjectWebCommon), "Stop")]

namespace SuperBob.App_Start
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using SuperBob.Repository;
    using SuperBob.Service;
    using SuperBob.Service.Contract;
    using SuperBob.Model;
    using System.Data.Entity;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));

            
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                kernel.Bind<DbContext>().To<SuperBobEntities>();
                kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
                kernel.Bind<IUserService>().To<UserService>();
                kernel.Bind<IRepository<Person>>().To<Repository<Person>>();
                kernel.Bind<IRepository<VideoLibrary>>().To <Repository<VideoLibrary>>();
                kernel.Bind<IVideoLibraryService>().To<VideoLibraryService>();
                kernel.Bind<IRepository<Genre>>().To<Repository<Genre>>();
                kernel.Bind<IGenreService>().To<GenreService>();
                
               // RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

         //  DbManager.RegisterFactory(kernel.Get<IUnitOfWork>());
          
        }        
    }
}
