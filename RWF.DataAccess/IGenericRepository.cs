#region

using System.Linq.Expressions;
using RWF.Model.Entities;

#endregion

namespace RWF.DataAccess;

public interface IGenericRepository<TEntity> where TEntity : EntityBase, new()
{
    IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    IEnumerable<TEntity> GetPage(int startRow, int pageLength,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    TEntity Get(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
    Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    void Load(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    Task LoadAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

    void Add(TEntity entity);

    TEntity Update(TEntity entity);

    void Remove(TEntity entity);
    void Remove(int id);

    bool Any(Expression<Func<TEntity, bool>> filter = null);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);

    int Count(Expression<Func<TEntity, bool>> filter = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
    Task SaveChanges();
}