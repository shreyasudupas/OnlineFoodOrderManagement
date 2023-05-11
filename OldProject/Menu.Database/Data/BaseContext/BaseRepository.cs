using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Mongo.Database.Data.BaseContext
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
        public Task Create(TEntity obj)
        {
            throw new NotImplementedException();
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
        public async Task<TEntity> GetByIdCustom(Expression<Func<TEntity, bool>> FindCondition) => await mongoCollection.Find(FindCondition).FirstOrDefaultAsync();

        public void Update(TEntity obj)
        {
            //mongoCollection.InsertOne(obj);
        }
    }
}
