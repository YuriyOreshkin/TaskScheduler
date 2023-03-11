using System;
using System.Threading.Tasks;

namespace TaskScheduler.Domain.Services.Interfaces
{
    public interface ITaskService
    {
        void START();
        void STOP();
        bool isSTARTED();
        void START(long TASKID);
        void STOP(long TASKID);
        DateTimeOffset? NEXTFIRETIME(long TASKID);
        bool isSTARTED(long TASKID);
    }
}
