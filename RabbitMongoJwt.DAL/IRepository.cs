using System;
using System.Collections.Generic;

namespace RabbitMongoJwt.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();

        void Add(TEntity entity);
    }
}
