using app.EntityModel;
using app.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Infrastructure.Repository
{
    public class EntityRepository<T> : IEntityRepository<T> where T : BaseEntity
    {
        private readonly IWorkContext workContext;
        protected readonly inventoryDbContext context;
        public EntityRepository(IWorkContext workContext, inventoryDbContext context)
        {
            this.context = context;
            this.workContext = workContext;
        }
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                entity = await GetAddAsyncProperties(entity);
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync(cancellationToken);

                return entity;
            }
            catch (Exception ex)
            {
                entity = null;
                return entity;
            }
        }

        public IQueryable<T> AllIQueryableAsync(CancellationToken cancellationToken = default)
        {
            return context.Set<T>().AsQueryable();
        }
        public async Task<T> FirstAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().FirstAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().ToListAsync(cancellationToken);
        }

        public Task<IReadOnlyList<T>> GetAllDeletedAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<IReadOnlyList<T>> GetAllPagedAsync(int recSkip, int recTake, CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().Where(s => s.IsActive == true).Skip(recSkip).Take(recTake).ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public Task<IReadOnlyList<T>> GetDeletedPagedAsync(int recSkip, int recTake, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PermanentDeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(true);
        }

        public async Task<bool> PermanentDeleteByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Remove(context.Set<T>().Find(id));
            await context.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity = await GetUpdateAsyncProperties(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(true);
        }
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().Where(s => s.IsActive == true).CountAsync();
        }

        public async Task<int> CountAsync()
        {
            return await context.Set<T>().Where(s => s.IsActive == true).CountAsync();
        }

        private async Task<T> GetUpdateAsyncProperties(T entity)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            entity.UpdatedOn = BaTime;
            entity.UpdatedBy = workContext.CurrentUserAsync().Result.FullName;
            return entity;
        }
        private async Task<T> GetAddAsyncProperties(T entity)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            entity.CreatedOn = BaTime;
            entity.CreatedBy = workContext.CurrentUserAsync().Result.FullName;
            entity.IsActive = true;
            entity.TrakingId= workContext.CurrentUserAsync().Result.Id;
            return entity;
        }

    }
}
