using Microsoft.EntityFrameworkCore.ChangeTracking;
using UcomGridView.Data.Entities;

namespace UcomGridView.Data.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity entity);
        ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAsync(int skip, int take);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
