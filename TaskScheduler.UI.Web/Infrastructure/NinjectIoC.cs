using Ninject;
using System.Web;
using TaskScheduler.Domain.EntityFramework;
using TaskScheduler.Domain.Services;
using TaskScheduler.Domain.Services.Interfaces;
using TaskScheduler.Domain.Services.Scheduler;

namespace TaskScheduler.UI.Web.Infrastructure
{
    public static class NinjectIoC
    {
        public static IKernel Initialize()
        {
            IKernel kernel = new StandardKernel();
            AddBindings(kernel);
            return kernel;
        }

        private static IKernel AddBindings(IKernel ninjectKernel)
        {
            //DI 
            ninjectKernel.Bind<IWorker>().To<HttpWorker>().InSingletonScope();
      
            //DI Logger
            ninjectKernel.Bind<ILogger>().To<NLogger>().InSingletonScope().WithConstructorArgument("_logname", HttpContext.Current.Server.MapPath("~/App_Data/log.txt"));


            //DI
            ninjectKernel.Bind<IRepository>().To<EFRepository>().InSingletonScope();

            //DI Task
            ninjectKernel.Bind<ITaskService>().To<QuartzSchedulerService>().InSingletonScope();

          

            return ninjectKernel;
        }
    }
}