﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Mongo.Database.Data.BaseContext
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        Task Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(string id);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAllItems();
    }

    public interface IEntity
    {
        public string Id { get; set; }
    }
}
