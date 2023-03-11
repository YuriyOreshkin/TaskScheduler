using System.Linq;

namespace TaskScheduler.Domain.Services.Interfaces
{
    public interface IRepository
    {
        IQueryable<TASK> GETTASKS();
        void ADDTASK(TASK task);
        void UPDATETASK(TASK task);
        void DELETETASK(TASK task);

    }
}
