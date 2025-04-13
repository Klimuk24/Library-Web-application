using System.Linq.Expressions;
using Library_Web_application.Data.Context;
using Library_Web_application.Data.Repository.Interfaces;
using Library_Web_application.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_application.Data.Repository;

public class BaseRepository<T>: IRepository<T> where T : class
{
    private LibraryDbContext Context { get; set; }
    protected readonly DbSet<T> DbSet;

    public BaseRepository(LibraryDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>(); 
    }
    
    public virtual IEnumerable<T> GetAll()
    {
        return DbSet;
    }

    public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression)
    {
        return DbSet.Where(expression);
    }

    public virtual T GetById(int id)
    {
        return DbSet.Find(id);
    }

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public void Save()
    {
        Context.SaveChanges();
    }
    
    public PagedResult<T> GetPaged(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNumber, int pageSize,
        Expression<Func<T, bool>> filter = null)
    {
        var query = DbSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var orderedQuery = orderBy(query);

        var result = new PagedResult<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = orderedQuery.Count(),
            Items = orderedQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList()
        };

        return result;
    }
}