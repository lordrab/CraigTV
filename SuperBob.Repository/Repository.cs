using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SuperBob.Model;
using System.Linq.Expressions;

namespace SuperBob.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private IUnitOfWork _unitOfWork;
        internal DbContext db;
        internal IDbSet<T> dbSet;
        internal DbEntityEntry<T> dbEntry;

        SuperBobEntities uow = new SuperBobEntities();
        
        public Repository(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            dbSet = uow.Set<T>();
            db = uow.GetDbContext();
        }
        public IEnumerable<T> GetAll()
        {
            
            return dbSet.AsEnumerable();
        }

        public T GetById(int id)
        {
            T model = dbSet.Find(id);
            return model;
        }

        public T Add(T model, bool persist = false)
        {
            dbSet.Add(model);
            _unitOfWork.Commit();

            return model;
        }

        public void Delete(int id)
        {
            T model = dbSet.Find(id);
            dbSet.Remove(model);
            _unitOfWork.Commit();
        }

        public void Update(T model)
        {
            var entry = db.Entry(model);
            if ( entry.State == EntityState.Detached )
            {
                //dbSet.Attach(model);
                db.Set<T>().Attach(model);
                entry.State = EntityState.Modified;
            }
            _unitOfWork.Commit();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).AsEnumerable();
        }
    }
}

