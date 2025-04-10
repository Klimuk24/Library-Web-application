using Library_Web_application.Domain.Entities;

namespace Library_Web_application.Data.Repository
{
    public interface IAuthorRepository : IDisposable
    {
        IEnumerable<Author> GetAuthorList(); // получение спискка всех авторов 
        IEnumerable<Book> GetAuthorBookList(int authorId); // получение спискка всех книг авторо 
        Author GetAuthorByID(int authorId); // получение автора по его ID
        void InsertAuthor(Author author); // добавление нового автора
        void UpdateAuthor(Author author); // изменение информации об авторе
        void DeleteAuthorByID(int authorId); // удаление автора
        void Save();  // сохранение изменений
    }
}
