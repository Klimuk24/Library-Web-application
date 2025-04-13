using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_application.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorController(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    // Получение всех авторов
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_authorRepository.GetAll());
    }

    // Получение автора по Id
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var author = _authorRepository.GetById(id);
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
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Author author)
    {
        if (id != author.Id) 
            return BadRequest("ID in URL and body don't match");
    
        var existingAuthor = _authorRepository.GetById(id);
        if (existingAuthor == null)
        {
            return NotFound($"Author with id {id} not found");
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
        var author = _authorRepository.GetById(id);
        if (author == null) return NotFound();
        
        _authorRepository.Delete(author);
        _authorRepository.Save();
        return NoContent();
    }
    
    // Тестовый GET для проверки пагинации 
    [HttpGet("paged")]
    public IActionResult GetPagedAuthors(
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = null)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Invalid pagination parameters");
        }

        var result = _authorRepository.GetPagedAuthors(pageNumber, pageSize, search);
        return Ok(result);
    }
}