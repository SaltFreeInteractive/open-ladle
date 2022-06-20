using Microsoft.EntityFrameworkCore;
using OpenLadle.Core.Abstractions;
using System.Linq.Expressions;

namespace OpenLadle.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    protected readonly ApplicationDbContext applicationDbContext;

    public Repository(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await applicationDbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> ListAsync()
    {
        return await applicationDbContext.Set<T>()
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate)
    {
        return await applicationDbContext.Set<T>()
            .Where(predicate)
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await applicationDbContext.Set<T>().AddAsync(entity);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        applicationDbContext.Set<T>().Remove(entity);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task EditAsync(T entity)
    {
        applicationDbContext.Entry(entity).State = EntityState.Modified;
        await applicationDbContext.SaveChangesAsync();
    }
}
