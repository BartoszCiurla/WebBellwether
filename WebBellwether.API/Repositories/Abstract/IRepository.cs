using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebBellwether.API.Repositories.Abstract
{
    public interface IRepository<T> where T:new()
    {
        List<T> Get(string language);
        T Get(int id,string language);
        List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        T Get(Expression<Func<T, bool>> predicate);
        T AsQueryable();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}