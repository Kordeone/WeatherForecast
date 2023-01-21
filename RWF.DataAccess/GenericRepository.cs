#region

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RWF.DataAccess.Query;
using RWF.Model.Entities;

#endregion

namespace RWF.DataAccess;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase, new()
{
    private readonly ForecastContext _context;
    private readonly DbSet<TEntity> _dbSet;
    private readonly OrderBy<TEntity> _defaultOrderBy = new OrderBy<TEntity>(qry => qry.OrderBy(e => e.Id));

    public GenericRepository(ForecastContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(null, orderBy, includes);
        return result.ToList();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(null, orderBy, includes);
        return await result.ToListAsync();
    }

    public IEnumerable<TEntity> GetPage(int startRow, int pageLength,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        if (orderBy == null) orderBy = _defaultOrderBy.Expression;

        var result = QueryDb(null, orderBy, includes);
        return result.Skip(startRow).Take(pageLength).ToList();
    }

    public async Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        if (orderBy == null) orderBy = _defaultOrderBy.Expression;

        var result = QueryDb(null, orderBy, includes);
        return await result.Skip(startRow).Take(pageLength).ToListAsync();
    }

    public TEntity Get(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }

        return query.SingleOrDefault(x => x.Id == id);
    }

    public Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null)
        {
            query = includes(query);
        }

        return query.SingleOrDefaultAsync(x => x.Id == id);
    }

    public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(filter, orderBy, includes);
        return result.ToList();
    }

    public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(filter, orderBy, includes);
        return await result.ToListAsync();
    }


    public IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        if (orderBy == null) orderBy = _defaultOrderBy.Expression;

        var result = QueryDb(filter, orderBy, includes);
        return result.Skip(startRow).Take(pageLength).ToList();
    }

    public async Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength,
        Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        if (orderBy == null) orderBy = _defaultOrderBy.Expression;

        var result = QueryDb(filter, orderBy, includes);
        return await result.Skip(startRow).Take(pageLength).ToListAsync();
    }

    public void Add(TEntity entity)
    {
        if (entity == null) throw new InvalidOperationException("Unable to add a null entity to the repository.");
        _dbSet.Add(entity);
    }

    public TEntity Update(TEntity entity)
    {
        return _dbSet.Update(entity).Entity;
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Deleted;
        _dbSet.Remove(entity);
    }

    public void Remove(int id)
    {
        var entity = new TEntity {Id = id};
        Remove(entity);
    }

    public bool Any(Expression<Func<TEntity, bool>> filter = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.Any();
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.AnyAsync();
    }

    public int Count(Expression<Func<TEntity, bool>> filter = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.Count();
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.CountAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task LoadAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(filter, orderBy, includes);
        await result.LoadAsync();
    }

    public void Load(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(filter, orderBy, includes);
        result.Load();
    }

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includes != null)
        {
            query = includes(query);
        }

        return query.SingleOrDefaultAsync();
    }

    public async Task LoadAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(null, orderBy, includes);
        await result.LoadAsync();
    }

    public void Load(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
    {
        var result = QueryDb(null, orderBy, includes);
        result.Load();
    }

    protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includes != null)
        {
            query = includes(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query;
    }
}