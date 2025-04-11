using Library_Web_application.Infrastructure.Enum;

namespace Library_Web_application.Data.Entities
{
    /// <summary>
    /// Сущность: Книга библиотеки
    /// </summary>
    public class Book
    {
        public int Id { get; set; } // Уникальный идентификатор книги
        public string ISBN { get; set; } // Номер книги в формате ISBN-13
        public string Title { get; set; } // Название книги
        
        public Genre Genre { get; set; } // Жанр книги
        
        public string? Description { get; set; } // Описание книги
        public int AuthorId { get; set; } // Идентификатор автора книги (внешний ключ)

        public DateTime? BorrowedTime { get; set; } // Дата и время, когда книга была взята читателем
        public DateTime? ReturnDueTime { get; set; } // Дата и время, когда книга должна быть возвращена

        public string? ImagePath { get; set; } // Путь к изображению книги
        public Author Author { get; set; } // Автор книги (навигационное свойство)
    }
}
