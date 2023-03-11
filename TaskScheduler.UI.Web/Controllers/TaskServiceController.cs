using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskScheduler.Domain;
using TaskScheduler.Domain.Services.Interfaces;
using TaskScheduler.UI.Web.Models;

namespace TaskScheduler.UI.Web.Controllers
{
    public class TaskServiceController : Controller
    {
        private  ITaskService service;
        public TaskServiceController(ITaskService _service)
        {
            service = _service;
        }


        public ActionResult Tasks()
        {

            return PartialView("~/Views/Scheduler/Tasks.cshtml", service.isSTARTED());
        }

        public JsonResult StartStopTask(TaskViewModel task)
        {
            //Run
            try
            {
                var message = "Задание '" + task.name + "' успешно ";
                if (task.isStarted)
                {

                    service.STOP(task.id);

                    message+="остановлено!";
                }
                else
                {
                    service.START(task.id);
                    message += "запущено!";

                }

                task.isStarted = service.isSTARTED(task.id);
                task.nextfiretime = ConvertModels.ConvertDate(service.NEXTFIRETIME(task.id));
                return Json(new { message = message, result = task }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new {  message = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> StartStopScheduler(bool isStarted)
        {
            //Run
            try
            {
                var message = "Сервис успешно ";
                if (isStarted)
                {

                    await Task.Run(() => service.STOP());

                    message += "остановлен!";
                }
                else
                {
                    await Task.Run(() =>
                        service.START()

                    );
                    message += "запущен!";

                }

                return Json(new { message = message, result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { message = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}