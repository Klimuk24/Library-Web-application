using Library_Web_application.Data.Context;
using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
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

        public override Author GetById(int id)
        {
            return DbSet.Include(a => a.Books).FirstOrDefault(a => a.Id == id);
        }
    }
}
