using LinqToDB;
using System.Linq.Expressions;

namespace LM.Data;
public class Repository<T>: IRepository<T> where T: class
{
    protected readonly  DbContext _ctx;

    public Repository(DbContext context)
    {
        _ctx = context;
    }

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
}