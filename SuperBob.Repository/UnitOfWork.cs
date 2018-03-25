using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace SuperBob.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;
        private Boolean _disposed = false;

        public UnitOfWork( DbContext context)
        {
            _dbContext = context;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if(!this._disposed)
            {
                _dbContext.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public DbEntityEntry<T> Entry<T>(T entity) where T : class
        {
            return _dbContext.Entry<T>(entity);
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }

        public IDbSet<T> Set<T>() where T : class
        {
            return _dbContext.Set<T>();
        }
    }
}
