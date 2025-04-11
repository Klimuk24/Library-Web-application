using Library_Web_application.Data.Context;
using Library_Web_application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_application.Data.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetBookList()
        {
            return _context.Books.Include(b => b.Author).ToList();
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == bookId);
        }

        public Book GetBookByISBN(string isbn)
        {
            return _context.Books.Include(b => b.Author).FirstOrDefault(b => b.ISBN == isbn);
        }

        public void InsertBook(Book book)
        {
            _context.Books.Add(book);
        }

        public void UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
        }

        public void DeleteBookById(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
                _context.Books.Remove(book);
        }

        public void CheckOutBookAsync(int bookId, int userId, DateTime dueDate)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                book.BorrowedTime = DateTime.Now;
                book.ReturnDueTime = dueDate;
                _context.Entry(book).State = EntityState.Modified;
            }
        }

        public void AddBookImage(int bookId, byte[] imageData, string contentType)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                // Здесь можно сохранить изображение в файловой системе или в базе данных
                // Для примера сохраняем путь к файлу
                var fileName = $"{bookId}_{DateTime.Now:yyyyMMddHHmmss}.{contentType.Split('/')[1]}";
                var filePath = Path.Combine("wwwroot/images", fileName);
                File.WriteAllBytes(filePath, imageData);
                book.ImagePath = $"/images/{fileName}";
                _context.Entry(book).State = EntityState.Modified;
            }
        }

        public void SendReminderMessage(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null && book.ReturnDueTime.HasValue && book.ReturnDueTime.Value < DateTime.Now)
            {
                // Здесь должна быть логика отправки уведомления
                // через email или систему сообщений
            }
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
