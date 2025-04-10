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

        // TODO -> Other functional:
        // Выдача книг на руки пользователю
        // Возможность добавления изображения к книге и его хранение
        // * Отправка уведомления об истечении срока выдачи книги (будет плюсом)
        void Save();  // сохранение изменений
    }
}
