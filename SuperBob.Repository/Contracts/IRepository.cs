using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace SuperBob.Repository
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Add(T model, bool persist = false);
        void Delete(int id);
        void Update(T model);
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    }
}
