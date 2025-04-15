using System.Linq.Expressions;
using Library_Web_application.Data.Context;
using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Library_Web_application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_application.Data.Repository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context) : base(context)
        {
        }
        
        public override IEnumerable<Author> GetAll()
        {
            return DbSet.Include(a => a.Books).ToList();
        }
        
        public PagedResult<Author> GetPagedAuthors(int pageNumber, int pageSize, string searchTerm = null)
        {
            Expression<Func<Author, bool>> filter = null;
    
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filter = a => a.FirstName.Contains(searchTerm) 
                              || a.LastName.Contains(searchTerm)
                              || a.Country.Contains(searchTerm);
            }

            return GetPaged(query => query.OrderBy(a => a.LastName),
                pageNumber, pageSize, filter
            );
        }
    }
}
