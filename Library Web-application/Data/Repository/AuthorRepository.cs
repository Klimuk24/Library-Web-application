using Library_Web_application.Data.Context;
using Library_Web_application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_application.Data.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Author> GetAuthorList()
        {
            return _context.Authors.ToList();
        }

        public IEnumerable<Book> GetAuthorBookList(int authorId)
        {
            return _context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId)
                .ToList();
        }

        public Author GetAuthorById(int authorId)
        {
            return _context.Authors.Find(authorId);
        }

        public void InsertAuthor(Author author)
        {
            _context.Authors.Add(author);
        }

        public void UpdateAuthor(Author author)
        {
            _context.Entry(author).State = EntityState.Modified;
        }

        public void DeleteAuthorById(int authorId)
        {
            var author = _context.Authors.Find(authorId);
            if (author != null)
                _context.Authors.Remove(author);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
