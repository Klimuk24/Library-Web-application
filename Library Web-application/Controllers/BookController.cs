using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_application.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookRepository _authorRepository;
    public BookController(IBookRepository bookRepository, IBookRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
    }

    // Получение всех книг
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_bookRepository.GetAll());
    }

    // Получение книги по Id
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _bookRepository.GetById(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    // Получение книги по ISBN
    [HttpGet("isbn/{isbn}")]
    public IActionResult GetByIsbn(string isbn)
    {
        var book = _bookRepository.GetByCondition(b => b.ISBN == isbn).FirstOrDefault();
        if (book == null) return NotFound();
        return Ok(book);
    }
    
    // Получение книг автора
    [HttpGet("{authorId}/books")]
    public IActionResult GetBooksByAuthor(int authorId)
    {
        var anyBookExists = _bookRepository.GetByCondition(b => b.AuthorId == authorId).Any();
    
        if (!anyBookExists)
        {
            var authorExists = _authorRepository.GetById(authorId) != null;
            if (!authorExists)
            {
                return NotFound($"Author with id {authorId} not found");
            }
        }
        var books = _bookRepository.GetByCondition(b => b.AuthorId == authorId).ToList();
        return Ok(books);
    }

    // Добавление книги
    [HttpPost]
    public IActionResult Create([FromBody] Book book)
    {
        if (_authorRepository.GetById(book.AuthorId) == null)
        {
            return BadRequest($"Author with id {book.AuthorId} not found");
        }
        
        book.Author = null!; 

        _bookRepository.Add(book);
        _bookRepository.Save();
    
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // Обновление книги
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Book book)
    {
        if (id != book.Id) return BadRequest();
        _bookRepository.Update(book);
        _bookRepository.Save();
        return NoContent();
    }

    // Удаление книги
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = _bookRepository.GetById(id);
        if (book == null) return NotFound();
        
        _bookRepository.Delete(book);
        _bookRepository.Save();
        return NoContent();
    }

    // Выдача книги
    [HttpPost("{id}/borrow")]
    public IActionResult BorrowBook(int id, [FromBody] DateTime returnDueDate)
    {
        try
        {
            _bookRepository.BorrowBook(id, returnDueDate);
            _bookRepository.Save();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Возврат книги
    [HttpPost("{id}/return")]
    public IActionResult ReturnBook(int id)
    {
        try
        {
            _bookRepository.ReturnBook(id);
            _bookRepository.Save();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    // Просроченные книги
    [HttpGet("overdue")]
    public IActionResult GetOverdueBooks()
    {
        return Ok(_bookRepository.GetOverdueBooks());
    }

    // Загрузка изображения
    [HttpPost("{id}/upload-image")]
    public IActionResult UploadImage(int id, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return BadRequest("Invalid file");

        try
        {
            _bookRepository.AddOrUpdateImage(id, imageFile);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}