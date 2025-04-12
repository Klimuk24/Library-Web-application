using Library_Web_application.Data.Context;
using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;

namespace Library_Web_application.Data.Repository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context) : base(context)
        {
        }
    }
}
