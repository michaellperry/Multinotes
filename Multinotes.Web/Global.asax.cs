using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Multinotes.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static SynchronizationService _synchronizationService = new SynchronizationService();

        public static SynchronizationService SynchronizationService
        {
            get { return _synchronizationService; }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SynchronizationService.Initialize();
        }
    }
}
