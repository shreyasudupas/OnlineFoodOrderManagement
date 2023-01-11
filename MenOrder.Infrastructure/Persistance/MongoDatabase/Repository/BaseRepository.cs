using MenuManagement.Infrastructure.Persistance.MongoDatabase.DbContext;
using MenuManagment.Domain.Mongo.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected IMongoCollection<TEntity> mongoCollection;
        private IMongoDBContext _context;
        public BaseRepository(IMongoDBContext context)
        {
            _context = context;
            mongoCollection = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        public async Task CreateOneDocument(TEntity document)
        {
            await mongoCollection.InsertOneAsync(document);
        }

        public async Task CreateManyDocument(ICollection<TEntity> documents)
        {
            await mongoCollection.InsertManyAsync(documents);
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllItems()
        {
            return await mongoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<TEntity> GetById(string id)
        {
            return await mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filterExpression) => await mongoCollection.Find(filterExpression).FirstOrDefaultAsync();

        public async Task<List<TEntity>> GetListByFilter(Expression<Func<TEntity, bool>> filterExpression) => await mongoCollection.Find(filterExpression).ToListAsync();

        public async Task<UpdateResult> UpdateOneDocument(FilterDefinition<TEntity> filter,UpdateDefinition<TEntity> update)
        {
            var result = await mongoCollection.UpdateOneAsync(filter,update);
            return result;
        }

        public int IfDocumentExists()
        {
            return mongoCollection.Find(_=> true).ToList().Count;
        }
    }
}
