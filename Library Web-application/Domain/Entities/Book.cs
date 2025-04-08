namespace Library_Web_application.Domain.Entities
{
    /// <summary>
    /// Сущность книги в библиотеке
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; } // Уникальный идентификатор книги
        public string ISBN { get; set; } // Номер книги в формате ISBN-13
        public string Title { get; set; } // Название книги
        public string Genre { get; set; } // Жанр книги
        public string Description { get; set; } // Описание книги

        public Guid AuthorId { get; set; } // Идентификатор автора книги (внешний ключ)
        public Author Author { get; set; } // Автор книги (навигационное свойство)

        public DateTime? BorrowedTime { get; set; } // Дата и время, когда книга была взята читателем
        public DateTime? ReturnDueTime { get; set; } // Дата и время, когда книга должна быть возвращена

        public string? ImagePath { get; set; } // Путь к изображению книги
    }
}
