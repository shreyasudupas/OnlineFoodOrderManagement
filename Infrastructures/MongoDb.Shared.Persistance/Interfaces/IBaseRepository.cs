using MenuManagment.Mongo.Domain.Mongo.Interfaces.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDb.Shared.Persistance.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateOneDocument(TEntity document);
        Task CreateManyDocument(ICollection<TEntity> documents);
        Task<UpdateResult> UpdateOneDocument(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update);
        Task<DeleteResult> DeleteOneDocument(FilterDefinition<TEntity> filter);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAllItems();
        public int IfDocumentExists();
        Task<TEntity> GetDocumentByFilter(Expression<Func<TEntity, bool>> filterExpression);
    }
}
