using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;
using TaskScheduler.Domain.Services.Interfaces;
using TaskScheduler.UI.Web.Models;

namespace TaskScheduler.UI.Web.Controllers
{
    public class LogSettingsController : Controller
    {
       
        private ILogger logger;

        public LogSettingsController(ILogger _logger)
        {
            logger = _logger;
        }
     
     
        public ActionResult Log()
        {

            return PartialView("~/Views/Settings/Log.cshtml");
        }

        public ActionResult ReadLog([DataSourceRequest]DataSourceRequest request, DateTime datebegin, DateTime dateend)
        {
            var log = logger.ReadLog(datebegin, dateend).Select(line =>ConvertModels.ConvertToViewModel(line, new LogStringViewModel())).OrderByDescending(d => d.date).ThenByDescending(t => t.time);

            JsonResult result = Json(log.ToDataSourceResult(request));
            result.MaxJsonLength = 8675309;


            return result;
        }

        
    }
}