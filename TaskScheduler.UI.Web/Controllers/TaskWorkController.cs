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
    public class TaskWorkController : Controller
    {
        private  IWorker service;
        public TaskWorkController(IWorker _service)
        {
            service = _service;
        }


        public async Task<JsonResult> RunTask(TaskViewModel task)
        {

            
            try
            {
                var entity = new TASK();
                entity = task.ToEntity(entity);
                await service.Do(entity);


            }
            catch (Exception exception)
            {
                return Json(new { message = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }



            return Json(new { result = task ,  message = "Задание '" + task.name + "' выполнено! " }, JsonRequestBehavior.AllowGet);
        }


    }
}