using Library_Web_application.Domain.Entities;

namespace Library_Web_application.Data.Repository
{
    public interface IAuthorRepository : IDisposable
    {
        IEnumerable<Author> GetAuthorList(); // получение спискка всех авторов 
        IEnumerable<Book> GetAuthorBookList(Author author); // получение спискка всех книг авторо 
        Book GetAuthorByID(int authorId); // получение автора по его ID
        void InsertAuthor(Author author); // добавление нового автора
        void UpdateAuthor(Author author); // изменение информации об авторе
        void DeleteAuthorByID(int authorId); // удаление автора по ID
        void Save();  // сохранение изменений
    }
}
