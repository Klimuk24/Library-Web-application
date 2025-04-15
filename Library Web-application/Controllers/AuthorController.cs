using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_application.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;

    public AuthorController(IAuthorRepository authorRepository, IBookRepository bookRepository)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
    }
    
    // Получение всех авторов
    [HttpGet]
    public IActionResult GetAll([FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, [FromQuery] string search = null)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Invalid pagination parameters");
        }

        var result = _authorRepository.GetPagedAuthors(pageNumber, pageSize, search);
        
        return Ok(result);
    }

    // Получение автора по Id
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var author = _authorRepository.GetSingle(x => x.Id == id);
        
        if (author == null) 
        {
            return NotFound(); 
        }
        
        return Ok(author);
    }
    
    // Добавление автора
    [HttpPost]
    public IActionResult Create([FromBody] Author author)
    {
        _authorRepository.Add(author);
        _authorRepository.Save();
        
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    // Обновление автора
    [HttpPut]
    public IActionResult Update([FromBody] Author author)
    {
        if (author == null || author.Id <= 0)
        {
            return BadRequest("Invalid author data");
        }

        var existingAuthor = _authorRepository.GetSingle(x => x.Id == author.Id);
    
        if (existingAuthor == null)
        {
            return NotFound($"Author with id {author.Id} not found");
        }
    
        existingAuthor.FirstName = author.FirstName;
        existingAuthor.LastName = author.LastName;
        existingAuthor.BirthDate = author.BirthDate;
        existingAuthor.Country = author.Country;

        _authorRepository.Update(existingAuthor); 
        _authorRepository.Save();

        return NoContent();
    }

    // Удаление автора
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var author = _authorRepository.GetSingle(x => x.Id == id);
        
        if (author == null)
        {
            return NotFound();
        }

        _authorRepository.Delete(author);
        _authorRepository.Save();
        
        return NoContent();
    }
    
    // Получение книг автора
    [HttpGet("{authorId}/books")]
    public IActionResult GetBooksByAuthor(int authorId)
    {
        var author = _authorRepository.GetSingle(a => a.Id == authorId);
        if (author == null)
        {
            return NotFound($"Author with id {authorId} not found");
        }
        
        var books = _bookRepository.GetByCondition(b => b.AuthorId == authorId)
            .Select(b => new {
                b.Id,
                b.Title,
                b.Isbn,
                b.Genre
            }).ToList();
        
        var result = new {
            Author = new {
                author.Id,
                author.FirstName,
                author.LastName
            },
            Books = books
        };

        return Ok(result);
    }
}