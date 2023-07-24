using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MenuManagment.Mongo.Domain.Mongo.Models;
using MongoDb.Shared.Persistance.DBContext;
using MongoDb.Shared.Persistance.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDb.Shared.Persistance.Repositories
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

        public async Task<DeleteResult> DeleteOneDocument(FilterDefinition<TEntity> filter)
        {
            return await mongoCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetAllItems()
        {
            return await mongoCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllItemsByPagination(Pagination mongoPagination)
        {
            return await mongoCollection.Find(_ => true).SortBy(x => x.Id).Skip(mongoPagination.Skip).Limit(mongoPagination.Limit).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllItemsByPaginationWithFilter(Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, object>> sortExpresion,
            bool asecending,
            Pagination mongoPagination)
        {
            if(asecending)
                return await mongoCollection.Find(filterExpression)
                    .SortBy(sortExpresion)
                    .Skip(mongoPagination.Skip)
                    .Limit(mongoPagination.Limit)
                    .ToListAsync();
            else
                return await mongoCollection.Find(filterExpression)
                    .SortByDescending(sortExpresion)
                    .Skip(mongoPagination.Skip)
                    .Limit(mongoPagination.Limit)
                    .ToListAsync();
        }

        public async Task<TEntity> GetById(string id)
        {
            return await mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filterExpression) => await mongoCollection.Find(filterExpression).FirstOrDefaultAsync();

        public async Task<List<TEntity>> GetListByFilter(Expression<Func<TEntity, bool>> filterExpression) => await mongoCollection.Find(filterExpression).ToListAsync();

        public async Task<List<TEntity>> GetListByFilterDefinition(FilterDefinition<TEntity> filterExpression) => await mongoCollection.Find(filterExpression).ToListAsync();

        public async Task<UpdateResult> UpdateOneDocument(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            var result = await mongoCollection.UpdateOneAsync(filter, update);
            return result;
        }

        public int IfDocumentExists()
        {
            return mongoCollection.Find(_ => true).ToList().Count;
        }

        public async Task<List<TEntity>> GetAllMatchItems(FilterDefinition<TEntity> filter)
        {
            return await mongoCollection.FindSync(filter).ToListAsync();
        }

        public async Task<string> CreateOneIndexAsync(CreateIndexModel<TEntity> indexModel)
        {
            var result = await mongoCollection.Indexes.CreateOneAsync(indexModel);
            return result;
        }

        public async Task CreateMultipleIndexAsync(List<CreateIndexModel<TEntity>> indexModels)
        {
            var result = await mongoCollection.Indexes.CreateManyAsync(indexModels);
        }
    }
}
