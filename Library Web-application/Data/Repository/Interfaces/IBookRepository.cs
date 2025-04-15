using Library_Web_application.Data.Entities;

namespace Library_Web_application.Data.Repository.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        void ReturnBook(int bookId);
        void AddOrUpdateImage(int bookId, IFormFile imageFile);
    }
}
