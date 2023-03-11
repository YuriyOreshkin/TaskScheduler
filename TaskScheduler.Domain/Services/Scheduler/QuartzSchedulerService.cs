using System.Linq;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using TaskScheduler.Domain.Services.Interfaces;
using Quartz.Impl.Matchers;

namespace TaskScheduler.Domain.Services.Scheduler
{
    public class QuartzSchedulerService : ITaskService, IDisposable
    {
        private IScheduler scheduler;
        private IJob job;
        private ILogger logger;
        IRepository repository;
        private bool disposed = false;


        public QuartzSchedulerService(IWorker _worker,IRepository _repository, ILogger _logger)
        {
            repository =_repository;
            job = new TaskWork(_worker);
            logger = _logger;

            START();
        }


        public  DateTimeOffset? NEXTFIRETIME(long taskid)
        {
            if (!scheduler.IsShutdown)
            {
                ITrigger trigger = scheduler.GetTrigger(new TriggerKey(taskid.ToString())).Result;

                if (trigger != null)
                {

                    return trigger.GetNextFireTimeUtc();
                }
            }
            return null;
        }

        private void StartJob(long taskid)
        {
            var task = repository.GETTASKS().FirstOrDefault(t => t.ID == taskid);
            IJobDetail jobDetail = CreateJobDetail(task);
            ITrigger trigger = CreateTrigger(task.SCHEDULER);

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        private IJobDetail CreateJobDetail(TASK task)
        {
            
            IJobDetail jobDetail = JobBuilder.Create<TaskWork>().WithIdentity(task.ID.ToString()).Build();
            JobDataMap dataMap = jobDetail.JobDataMap;
            dataMap["task"] = task;

            return jobDetail;
        }

        private ITrigger CreateTrigger(SCHEDULER _scheduler)
        {

            var time = new TimeSpan(0, 0, 0);


            if (!String.IsNullOrEmpty(_scheduler.STARTINGDAILYAT)) {

                TimeSpan.TryParse(_scheduler.STARTINGDAILYAT, out time);
            }

            var startAt = TimeOfDay.HourAndMinuteOfDay(time.Hours, time.Minutes);

            time = new TimeSpan(23, 59, 59);

            if (!String.IsNullOrEmpty(_scheduler.ENDINGDAILYAT))
            {
                TimeSpan.TryParse(_scheduler.ENDINGDAILYAT, out time);
               
            }

            var endAt = TimeOfDay.HourAndMinuteOfDay(time.Hours, time.Minutes);

            ITrigger trigger = TriggerBuilder.Create()
                   .WithIdentity(_scheduler.ID.ToString())
                   .StartAt(new DateTimeOffset())
                   .StartNow()
                   //.WithSimpleSchedule(x => x.WithIntervalInMinutes(settings.INTERVALINMINUTES).RepeatForever())
                   .WithDailyTimeIntervalSchedule(x=>x.OnDaysOfTheWeek(DaysOfWeeks(_scheduler.ONDAYSOFTHEWEEK).ToArray()).WithIntervalInMinutes(_scheduler.INTERVALINMINUTES).StartingDailyAt(startAt).EndingDailyAt(endAt))
                   .Build();

            return trigger;
        }

        private List<DayOfWeek> DaysOfWeeks(string setting)
        {
            List<DayOfWeek> dayOfWeeks = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            if (!String.IsNullOrEmpty(setting))
            {
                dayOfWeeks = dayOfWeeks.Where(x => setting.Split('|').Contains((dayOfWeeks.IndexOf(x) + 1).ToString())).ToList();
            }

            return dayOfWeeks;
        }



        private async void StartJobs()
        {
            var jobs = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
            foreach (TASK task in repository.GETTASKS())
            {
                if (task.ENABLE)
                {
                    IJobDetail jobDetail = CreateJobDetail(task);
                    ITrigger trigger = CreateTrigger(task.SCHEDULER);

                    jobs.Add(jobDetail, new List<ITrigger>() { trigger });
                }
            }

            await scheduler.ScheduleJobs(jobs, true);
        }



        public void START(long taskid)
        {
            TASK task = repository.GETTASKS().FirstOrDefault(t => t.ID == taskid); 
            StartJob(taskid);
            task.ENABLE = true;
            repository.UPDATETASK(task);
            logger.Info(task.NAME + "|Запущено!");
           
        }

        public void STOP(long taskid)
        {
            TASK task = repository.GETTASKS().FirstOrDefault(t => t.ID == taskid);
            var job = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result.FirstOrDefault(j=>j.Name == taskid.ToString());
            if (job != null)
            {
                scheduler.DeleteJob(job);

                task.ENABLE = false;
                repository.UPDATETASK(task);

                logger.Info(task.NAME+ "|Остановлено!");
            }
            else {

                logger.Info(task.NAME+ "|Расписание не найдено!");
            }
        }

        public bool isSTARTED(long taskid)
        {
            if (!scheduler.IsShutdown)
            {
                var job = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result.FirstOrDefault(t => t.Name == taskid.ToString());
                if (job != null)
                {
                    return true;
                }

                return false;
            }

            return false;
           
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    scheduler.Shutdown();
                }
            }
            this.disposed = true;
        }

        public async void START()
        {
            scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            scheduler.JobFactory = new JobFactory(job);

            await scheduler.Start();

            StartJobs();

            logger.Info("Сервис|Запущен!");
        }

        public async void STOP()
        {
            if (scheduler != null)
            {
                await scheduler.Shutdown();
                logger.Info("Сервис|Остановлен!");
            }
        }

        public bool isSTARTED()
        {
            if (scheduler != null)
            {
                if (!scheduler.IsShutdown)
                {
                    return scheduler.IsStarted;
                }
                return false;
            }

            return false;
        }
    }
}
