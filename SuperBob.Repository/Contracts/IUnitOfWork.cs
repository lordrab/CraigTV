using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace SuperBob.Repository
{
    public interface IUnitOfWork
    {
        DbContext GetDbContext();
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        IDbSet<T> Set<T>() where T : class;
        void Commit();
        void Dispose();

    }
    
}
