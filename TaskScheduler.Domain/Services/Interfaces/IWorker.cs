using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.Domain;

namespace TaskScheduler.Domain.Services.Interfaces
{
    public interface IWorker
    {
        Task Do(TASK task);
    }
}
