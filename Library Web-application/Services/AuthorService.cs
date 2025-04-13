using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Library_Web_application.Services.Interfaces;

namespace Library_Web_application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public Author GetById(int id)
    {
        return _authorRepository.GetById(id);
    }

    public IEnumerable<Author> GetAll()
    {
        return _authorRepository.GetAll();
    }

    public void Create(Author author)
    {
        if (author == null)
            throw new ArgumentNullException(nameof(author));

        _authorRepository.Add(author);
        _authorRepository.Save();
    }

    public void Update(Author author)
    {
        if (author == null)
            throw new ArgumentNullException(nameof(author));

        _authorRepository.Update(author);
        _authorRepository.Save();
    }

    public void Delete(int id)
    {
        var author = _authorRepository.GetById(id);
        if (author == null)
            throw new KeyNotFoundException($"Author with id {id} not found");

        _authorRepository.Delete(author);
        _authorRepository.Save();
    }
}