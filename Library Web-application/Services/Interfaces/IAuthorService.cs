using Library_Web_application.Data.Entities;

namespace Library_Web_application.Services.Interfaces;

public interface IAuthorService
{
    Author GetById(int id);
    IEnumerable<Author> GetAll();
    void Create(Author author);
    void Update(Author author);
    void Delete(int id);
}