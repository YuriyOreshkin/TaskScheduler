using System.Data.Entity;

namespace TaskScheduler.Domain.Services.EntityFramework
{
    public class DBContext : DbContext
    {
        public DBContext(): base("TaskScheduler")
        {

        }
        public DbSet<TASK> TASKS { get; set; }
        public DbSet<SCHEDULER> SCHEDULERS { get; set; }
    }
}
