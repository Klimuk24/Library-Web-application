using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Library_Web_application.Infrastructure.Enum;
using Library_Web_application.Infrastructure.Models;
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
        var book = _bookRepository.GetSingle(x => x.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book); 
    }

    // Получение книги по ISBN
    [HttpGet("isbn/{isbn}")]
    public IActionResult GetByIsbn(string isbn)
    {
        var book = _bookRepository.GetSingle(b => b.Isbn == isbn);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    // Добавление книги
    [HttpPost]
    public IActionResult Create([FromBody] Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (book.AuthorId <= 0)
        {
            return BadRequest("AuthorId is required");
        }
    
        var author = _authorRepository.GetSingle(x => x.Id == book.AuthorId);
    
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
            BorrowedTime = book.BorrowedTime,
            ReturnDueTime = book.ReturnDueTime
        };

        _bookRepository.Add(newBook);
        _bookRepository.Save();
    
        return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
    }

    // Обновление книги
    [HttpPut]
    public IActionResult Update([FromBody] Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (book.Id <= 0)
        {
            return BadRequest("Book Id is required");
        }

        var existingBook = _bookRepository.GetSingle(x => x.Id == book.Id);
    
        if (existingBook == null)
        {
            return NotFound($"Book with id {book.Id} not found");
        }
        
        if (existingBook.AuthorId != book.AuthorId)
        {
            var newAuthor = _authorRepository.GetSingle(x => x.Id == book.AuthorId);
        
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
        existingBook.BorrowedTime = book.BorrowedTime;
        existingBook.ReturnDueTime = book.ReturnDueTime;

        _bookRepository.Update(existingBook);
        _bookRepository.Save();

        return NoContent();
    }

    // Удаление книги
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = _bookRepository.GetSingle(x => x.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        _bookRepository.Delete(book);
        _bookRepository.Save();
        
        return Ok();
    }
    
    // Выдача книги
    [HttpPost("/borrow")]
    public IActionResult BorrowBook([FromBody] BorrowModel model)
    {
        var book = _bookRepository.GetSingle(x => x.Id == model.BookId);
        
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with id {model.BookId} not found");
        }

        if (book.BorrowedTime != null)
        {
            throw new InvalidOperationException("Book is already borrowed");
        }

        book.BorrowedTime = DateTime.Now;
        book.ReturnDueTime = model.ReturnDueDate;
        
        _bookRepository.Update(book);
        _bookRepository.Save();
        
        return Ok();
    }

    // Возврат книги
    [HttpPost("{id}/return")]
    public IActionResult ReturnBook(int id)
    {
        _bookRepository.ReturnBook(id);
        _bookRepository.Save();
        
        return Ok();
    }
    
    // Получение списка просроченных книг
    [HttpGet("overdue")]
    public IActionResult GetOverdueBooks()
    {
        var books = _bookRepository.GetByCondition(b => 
            b.ReturnDueTime != null && b.ReturnDueTime < DateTime.Now).ToList();
        
        return Ok(books);
    }

    // Загрузка изображения
    [HttpPost("{id}/image")]
    public IActionResult UploadImage(int id, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        _bookRepository.AddOrUpdateImage(id, imageFile);
        
        return Ok();
    }
}