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
    /// Проверка на сучествоание
    /// </summary>
    /// <returns></returns>
    public Task<bool> AnyAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить по условию
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
    /// <returns></returns>
    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///  Удалть выборку
    /// </summary>
    /// <param name="func"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task DeleteAsync(Expression<Func<T, bool>> func, CancellationToken cancellationToken = default);

    /// <summary>
    /// Вставить набор данных
    /// </summary>
    /// <param name="items"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task SaveManyAsync(IEnumerable<T> items, CancellationToken cancellationToken = default);
}