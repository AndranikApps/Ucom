using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UcomGridView.Data.Contexts;
using UcomGridView.Data.Entities;
using UcomGridView.Data.Interfaces;

namespace UcomGridView.Data.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseContext _databaseContext;
        private DbSet<TEntity> _entity;

        public GenericRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _entity = _databaseContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _entity.Add(entity);
        }

        public async ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return await _entity.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(int skip, int take)
        {
            return await _entity.AsNoTracking().Skip(skip).Take(take).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _entity.SingleOrDefaultAsync(x => x.Id == id);
        }

        public void Update(TEntity entity)
        {
            _entity.Update(entity);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
