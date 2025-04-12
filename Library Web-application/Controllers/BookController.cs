using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Library_Web_application.Infrastructure.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_application.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IWebHostEnvironment _environment;

    public BookController(IBookRepository bookRepository, IWebHostEnvironment environment)
    {
        _bookRepository = bookRepository;
        _environment = environment;
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
    public IActionResult GetByISBN(string isbn)
    {
        var book = _bookRepository.GetByCondition(b => b.ISBN == isbn).FirstOrDefault();
        if (book == null) return NotFound();
        return Ok(book);
    }
    
    // Получение книг автора
    [HttpGet("{authorId}/books")]
    public IActionResult GetBooksByAuthor(int authorId)
    {
        // Используем GetByCondition для фильтрации книг по автору
        var books = _bookRepository.GetByCondition(b => b.AuthorId == authorId);
        return Ok(books);
    }

    // Добавление книги
    [HttpPost]
    public IActionResult Create([FromBody] Book book)
    {
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

    // Просроченные книги
    [HttpGet("overdue")]
    public IActionResult GetOverdueBooks()
    {
        return Ok(_bookRepository.GetOverdueBooks());
    }

    // Поиск по названию/жанру
    [HttpGet("search")]
    public IActionResult Search(string title, Genre? genre)
    {
        var books = _bookRepository.GetByCondition(b =>
            (string.IsNullOrEmpty(title) || b.Title.Contains(title)) &&
            (!genre.HasValue || b.Genre == genre.Value)
        ).ToList();
        
        return Ok(books);
    }
}