using Library_Web_application.Data.Entities;

namespace Library_Web_application.Services.Interfaces;

public interface IAuthorService
{
    IEnumerable<Author> GetAll();
    Author GetById(int id);
    void Create(Author author);
    void Update(Author author);
    void Delete(int id);
}