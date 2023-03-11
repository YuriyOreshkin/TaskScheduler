using Quartz;
using Quartz.Spi;

namespace TaskScheduler.Domain.Services.Scheduler
{
    public class JobFactory : IJobFactory
    {
        IJob job;
        public JobFactory(IJob _job)
        {
            job = _job;
        } 
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return job;
        }

        public void ReturnJob(IJob job)
        {
            //this.ninjectKernel.Release(job);
        }

       
    }
}
