using Microsoft.EntityFrameworkCore;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Data.EntityFramework.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly AppDbContext _dbContext;

    public EfRepository(AppDbContext dbContext)
    {
        if (dbContext is null)
            throw new ArgumentNullException(nameof(dbContext));
        _dbContext = dbContext;
    }
    
    protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

    public virtual async Task<TEntity> GetById(Guid id, CancellationToken cancellationToken)
        => await Entities.FirstAsync(it => it.Id == id, cancellationToken);

    public virtual async Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken)
        => await Entities.ToListAsync(cancellationToken);
    
    public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));
        
        await Entities.AddAsync(entity, cancellationToken);
    }
    
    public virtual async Task Update(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));
        
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual async Task Delete(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        
        Entities.Remove(entity);
    }
}