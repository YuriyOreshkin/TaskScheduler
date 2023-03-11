using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskScheduler.Domain;
using TaskScheduler.Domain.Services.Interfaces;

namespace TaskScheduler.UI.Web.Models
{
    public static class ConvertModels
    {
        public static TaskViewModel ConvertToViewModel(TASK entity, TaskViewModel view, ITaskService service)
        {
            view.id = entity.ID;
            view.name = entity.NAME;
            view.intervalinminutes = entity.SCHEDULER.INTERVALINMINUTES;
            view.startingdailyat = DateTime.Now.Date + ConvertTime(entity.SCHEDULER.STARTINGDAILYAT);
            view.endingdailyat = DateTime.Now.Date + ConvertTime(entity.SCHEDULER.ENDINGDAILYAT);
            view.isStarted = service.isSTARTED(entity.ID);
            view.ondaysoftheweek = entity.SCHEDULER.ONDAYSOFTHEWEEK.Split('|');
            view.nextfiretime = ConvertDate(service.NEXTFIRETIME(entity.ID));
            view.job = entity.JOB;
            return view;
        }

        public static LogStringViewModel ConvertToViewModel(string line, LogStringViewModel view)
        {
            string[] items = line.Split('|');
            view.date = DateTime.Parse(items[0]).Date;
            view.time = DateTime.Parse(items[0]).TimeOfDay;
            view.type = items[1];
            if (items.Count() > 4)
            {
                view.task = items[3];
                view.content = items[4];
            }
            else {
                view.content = items[3];
            }
            return view;
        }

        public static DateTime? ConvertDate(DateTimeOffset? date)
        {
            if (date.HasValue)
            {
                return date.Value.DateTime.ToLocalTime();
            }
            else
            {
                return null;
            }
        }
        private static TimeSpan ConvertTime(string time_string)
        {
            TimeSpan time = new TimeSpan(0, 0, 0);
            TimeSpan.TryParse(time_string, out time);

            return time;
        }
    }
}