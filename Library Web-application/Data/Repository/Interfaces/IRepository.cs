using System.Linq.Expressions;
using Library_Web_application.Infrastructure.Models;

namespace Library_Web_application.Data.Repository.Interfaces;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(); // получение спискка всех объектов
    IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression); // получение спискка всех объектов по условию
    T? GetSingle(Expression<Func<T, bool>> expression); // получение объекта по ID
    void Add(T entity); // добавление нового объекта
    void Update(T entity); // изменение информации об объекте
    void Delete(T entity); // удаление объекта
    void Save();  // сохранение изменений
    PagedResult<T> GetPaged(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNumber, int pageSize, 
        Expression<Func<T, bool>> filter = null); // Пагинация данных 
}

