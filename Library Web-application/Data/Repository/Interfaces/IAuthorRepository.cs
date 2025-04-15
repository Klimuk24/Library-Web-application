using Library_Web_application.Data.Entities;
using Library_Web_application.Infrastructure.Models;

namespace Library_Web_application.Data.Repository.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        PagedResult<Author> GetPagedAuthors(int pageNumber, int pageSize, string searchTerm = null);
    }
}
