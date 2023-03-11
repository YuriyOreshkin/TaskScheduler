using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaskScheduler.UI.Web.Infrastructure;

namespace TaskScheduler.UI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
       

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Ninject 
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

        }
    }
}
