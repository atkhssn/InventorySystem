using app.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Infrastructure.Repository
{
    public interface IEntityRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllDeletedAsync(CancellationToken cancellationToken = default);
        IQueryable<T> AllIQueryableAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllPagedAsync(int recSkip, int recTake, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetDeletedPagedAsync(int recSkip, int recTake, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> PermanentDeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> PermanentDeleteByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<T> FirstAsync(CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync();
    }
}
