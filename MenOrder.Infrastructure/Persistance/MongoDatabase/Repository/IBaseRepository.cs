using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MenuManagement.Infrastructure.Persistance.MongoDatabase.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateOneDocument(TEntity document);
        Task CreateManyDocument(ICollection<TEntity> documents);
        void Update(TEntity obj);
        void Delete(string id);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAllItems();
        public int IfDocumentExists();

        Task<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filterExpression);

    }

    public interface IEntity
    {
        public string Id { get; set; }
    }
}
