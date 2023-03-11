using System;
using System.Data.Entity;
using System.Linq;
using TaskScheduler.Domain.Services.EntityFramework;
using TaskScheduler.Domain.Services.Interfaces;

namespace TaskScheduler.Domain.EntityFramework
{
    public class EFRepository : IRepository, IDisposable
    {
        private DBContext db;
        private bool disposed = false;

        public EFRepository()
        {
            db = new DBContext();
           
        } 

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    

        public IQueryable<TASK> GETTASKS()
        {
            return db.TASKS.Include("SCHEDULER");
        }

        public void ADDTASK(TASK task)
        {
            db.TASKS.Add(task);
            db.SaveChanges();
        }

        public void UPDATETASK(TASK task)
        {
            var entity = GETTASKS().FirstOrDefault(t => t.ID == task.ID);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DELETETASK(TASK task)
        {
            
            TASK entity = GETTASKS().FirstOrDefault(t => t.ID == task.ID);
            if (entity != null)
            {
                db.TASKS.Remove(entity);
                db.SaveChanges();

            }

        }
    }
}
