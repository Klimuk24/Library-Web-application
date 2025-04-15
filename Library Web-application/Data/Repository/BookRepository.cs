using System.Linq.Expressions;
using Library_Web_application.Infrastructure.Models;
using Library_Web_application.Data.Context;
using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;

namespace Library_Web_application.Data.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly IWebHostEnvironment _environment;

        public BookRepository(LibraryDbContext context, IWebHostEnvironment environment) : base(context)
        {
            _environment = environment;
        }
        
        public void ReturnBook(int bookId)
        {
            var book = GetSingle(x => x.Id == bookId);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {bookId} not found");

            book.BorrowedTime = null;
            book.ReturnDueTime = null;

            Save();
        }
        
        public void AddOrUpdateImage(int bookId, IFormFile imageFile)
        {
            var book = GetSingle(x => x.Id == bookId);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {bookId} not found");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "books");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{bookId}_{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            if (!string.IsNullOrEmpty(book.ImagePath))
            {
                var oldFilePath = Path.Combine(_environment.WebRootPath, book.ImagePath.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            book.ImagePath = $"/images/books/{fileName}";
            Save();
        }
    }
}
