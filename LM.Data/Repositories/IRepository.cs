using System.Linq.Expressions;

namespace LM.Data;
/// <summary>
/// Репозиторий
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Получить всё
    /// </summary>
    /// <returns></returns>
    public Task<T[]> ReadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<T?> Get(Expression<Func<T, bool>> func, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Task<int> SaveAsync(T entity, CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Удалить
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}