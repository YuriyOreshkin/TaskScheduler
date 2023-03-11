using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskScheduler.Domain;
using TaskScheduler.Domain.Services.Interfaces;
using TaskScheduler.UI.Web.Extensions;
using TaskScheduler.UI.Web.Models;

namespace TaskScheduler.UI.Web.Controllers
{
    public class TaskDirectoryController : Controller
    {
        private  IRepository repository;
        private  ITaskService service;
        public TaskDirectoryController(IRepository _repository, ITaskService _service)
        {
            repository = _repository;
            service = _service;
        }


        /// <summary>
        /// TaskDirectory/Settings?name=Задание1
        /// </summary>
        /// <param name="name">Task name</param>
        /// <returns></returns>
        [AllowCrossSite]
        public ActionResult Settings(string name)
        {
            var task = repository.GETTASKS().FirstOrDefault(t=>t.NAME == name);
            if (task != null)
            {
                return PartialView("~/Views/Settings/SchedulerSettings.cshtml", ConvertModels.ConvertToViewModel(task, new TaskViewModel(), service));
            }

            return Content(String.Format("Настройки расписания для задания {0} не найдены",name));
        } 


        public ActionResult ReadGridView([DataSourceRequest] DataSourceRequest request)
        {
            var tasks = repository.GETTASKS().ToList().Select(t => ConvertModels.ConvertToViewModel(t, new TaskViewModel(),service)).ToList();


            return Json(tasks.ToDataSourceResult(request));
        }

        public JsonResult ReadForDropDownList()
        {
            var tasks = repository.GETTASKS().ToList().Select(t => new SelectListItem { Text = t.NAME, Value = t.ID.ToString() }).ToList();
            tasks.Add(new SelectListItem { Text = "Сервис", Value = "0" });
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        //Create
        [HttpPost]
        public ActionResult CreateForGrid([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = task.ToEntity(new TASK());
                    repository.ADDTASK(entity);
                    task.id = entity.ID;

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", ex.Message);
                }
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult UpdateForGrid([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                var entity = repository.GETTASKS().FirstOrDefault(f => f.ID == task.id);
                if (entity != null)
                {
                    try
                    {

                        entity = task.ToEntity(entity);
                        //STOP when UPDATE
                        service.STOP(task.id);

                        repository.UPDATETASK(entity);

                        task.isStarted = service.isSTARTED(task.id);
                        task.nextfiretime = null;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("error",  ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("error", String.Format("Задание {0} не обнаружено в базе данных", task.name));
                }

            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));

        }


        //Delete
        [HttpPost]
        public ActionResult DestroyForGrid([DataSourceRequest]DataSourceRequest request, TaskViewModel task)
        {
            var entity = repository.GETTASKS().FirstOrDefault(f => f.ID == task.id);
            if (entity != null)
            {
                try
                {
                    service.STOP(task.id);
                    repository.DELETETASK(entity);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("error", ex.Message);
                }

            }
            else
            {
                ModelState.AddModelError("error", String.Format("Задание {0} не обнаружено в базе данных", task.name));
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));

        }



        private TimeSpan ConvertTime(string time_string)
        {
            TimeSpan time = new TimeSpan(0, 0, 0);
            TimeSpan.TryParse(time_string, out time);

            return time;
        }

        


    }
}