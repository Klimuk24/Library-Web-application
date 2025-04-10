using Library_Web_application.Domain.Entities;

namespace Library_Web_application.Data.Repository
{
    public interface IBookRepository : IDisposable
    {
        IEnumerable<Book> GetBookList(); // получение списка всех книг
        Book GetBookByID(int bookId); // получение одной книги по ID
        Book GetBookByISBN(string isbn); // получение одной книги по ISBN
        void InsertBook(Book book); // добавление новой книги
        void UpdateBook(Book book); // обновление ифнормации об книге
        void DeleteBookByID(int bookId); // удаление книги по ID
        void CheckOutBookAsync(int bookId, int userId, DateTime dueDate); // Выдача книг на руки пользователю
        void AddBookImage(int bookId, byte[] imageData, string contentType); // Возможность добавления изображения к книге и его хранение
        void SendReminderMessage(int bookId); // * Отправка уведомления об истечении срока выдачи книги 
        void Save();  // сохранение изменений
    }
}
