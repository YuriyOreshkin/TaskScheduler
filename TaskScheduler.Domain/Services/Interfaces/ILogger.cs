using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskScheduler.Domain.Services.Interfaces
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        IEnumerable<string> ReadLog(DateTime datebegin, DateTime dateend);
    }
}
