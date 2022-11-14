using LinqToDB;
using System.Linq.Expressions;
using LinqToDB.Data;

namespace LM.Data;
public class Repository<T>: IRepository<T> where T: class
{
    protected readonly  DbContext _ctx;

    public Repository(DbContext context)
    {
        _ctx = context;
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken = default)
        => _ctx.GetTable<T>().AnyAsync(func, cancellationToken);

    public Task<T?> Get(Expression<Func<T, bool>> func, CancellationToken cancellationToken = default) 
        => _ctx.GetTable<T>().FirstOrDefaultAsync(func, cancellationToken);

    public Task<T[]> ReadAsync(CancellationToken cancellationToken = default)
    {
        return _ctx.GetTable<T>().ToArrayAsync(cancellationToken);
    }

    public Task<int> SaveAsync(T entity, CancellationToken cancellationToken = default)
    {
        return _ctx.InsertOrReplaceAsync(entity, token: cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        return _ctx.DeleteAsync(entity, token: cancellationToken);
    }
    
    public Task DeleteAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken = default)
    {
        return _ctx.GetTable<T>()
            .Where(func)
            .DeleteAsync(token: cancellationToken);
    }
    
    public Task SaveManyAsync(IEnumerable<T> items, CancellationToken cancellationToken = default)
    {
        return _ctx.BulkCopyAsync(items, cancellationToken);
    }
}