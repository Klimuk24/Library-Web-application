using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_application.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository)
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
        var book = _bookRepository.GetByCondition(b => b.Isbn == isbn).FirstOrDefault();
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var author = _authorRepository.GetById(book.AuthorId);
        if (author == null)
        {
            return BadRequest($"Author with id {book.AuthorId} not found");
        }
        
        var newBook = new Book
        {
            Isbn = book.Isbn,
            Title = book.Title,
            Description = book.Description,
            Genre = book.Genre,
            AuthorId = book.AuthorId,
            BorrowedTime = null,
            ReturnDueTime = null
        };

        _bookRepository.Add(newBook);
        _bookRepository.Save();
        
        var createdBook = _bookRepository.GetById(newBook.Id);
        return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, createdBook);
    }

    // Обновление книги
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != book.Id)
        {
            return BadRequest("ID in URL and body don't match");
        }

        var existingBook = _bookRepository.GetById(id);
        if (existingBook == null)
        {
            return NotFound($"Book with id {id} not found");
        }
        
        if (existingBook.AuthorId != book.AuthorId)
        {
            var newAuthor = _authorRepository.GetById(book.AuthorId);
            if (newAuthor == null)
            {
                return BadRequest($"Author with id {book.AuthorId} not found");
            }
        }
        
        existingBook.Isbn = book.Isbn;
        existingBook.Title = book.Title;
        existingBook.Description = book.Description;
        existingBook.Genre = book.Genre;
        existingBook.AuthorId = book.AuthorId;

        _bookRepository.Update(existingBook);
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