using Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace RabbitMongoJwt.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IAppSettings appSettings)
        {
            var database = new MongoClient().GetDatabase(appSettings.Databasename);
            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);

        }
        public void Add(TEntity entity)
        {
            _collection.InsertOne(entity);
        }

        public TEntity Get(Guid id)
        {
            var filter = Builders<TEntity>.Filter.Eq("Id", id);

            var result = _collection.Find(filter).FirstOrDefault();

            return result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            var result = _collection.Find(new BsonDocument()).ToList();
            return result;
        }
    }
}
