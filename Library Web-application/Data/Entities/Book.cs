using System.Text.Json.Serialization;
using Library_Web_application.Infrastructure.Enum;

namespace Library_Web_application.Data.Entities
{
    /// <summary>
    /// Сущность: Книга библиотеки
    /// </summary>
    public class Book
    {
        public int Id { get; set; } // ID книги
        public required string Isbn { get; set; } // Номер книги в формате ISBN-13
        public required string Title { get; set; } // Название книги
        public string? Description { get; set; } // Описание книги
        public string? ImagePath { get; set; } // Путь к изображению книги
        public Genre Genre { get; set; } // Жанр книги
        public DateTime? BorrowedTime { get; set; } // Дата и время, когда книга была взята читателем
        public DateTime? ReturnDueTime { get; set; } // Дата и время, когда книга должна быть возвращена
        public required int AuthorId { get; set; } // ID автора книги (внешний ключ)
        [JsonIgnore] public Author? Author { get; set; } // Автор книги (навигационное свойство)
    }
}
