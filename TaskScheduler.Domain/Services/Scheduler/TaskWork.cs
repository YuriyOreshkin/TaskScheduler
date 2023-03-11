using Quartz;
using System.Threading.Tasks;
using TaskScheduler.Domain.Services.Interfaces;

namespace TaskScheduler.Domain.Services.Scheduler
{
    public class TaskWork : IJob
    {
        private IWorker worker;

        public TaskWork(IWorker _worker)
        {
            worker = _worker;
        } 

        public Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            object value;
            TASK task=null;
            if (dataMap.TryGetValue("task", out value))
            {
                task = value as TASK;
            }

            return worker.Do(task);
           
        }
    }
}
