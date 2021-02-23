using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PluralVideos.Data.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BaseContext Context;

        public Repository(BaseContext context)
        {
            Context = context;
        }

        public TEntity Get(int id) 
            => Context.Set<TEntity>().Find(id);

        public async Task<IEnumerable<TEntity>> GetAllAsync() 
            => await Context.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await Context.Set<TEntity>().Where(predicate).ToListAsync();

        public void Add(TEntity entity)
            => Context.Set<TEntity>().Add(entity);

        public void AddRange(IEnumerable<TEntity> entities)
            => Context.Set<TEntity>().AddRange(entities);

        public void Remove(TEntity entity)
            => Context.Set<TEntity>().Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities)
            => Context.Set<TEntity>().RemoveRange(entities);
    }
}
