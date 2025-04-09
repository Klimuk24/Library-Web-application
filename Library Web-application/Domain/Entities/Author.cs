namespace Library_Web_application.Domain.Entities
{
    /// <summary>
    /// Сущность автора книги
    /// </summary>
    public class Author
    {
        public Guid Id { get; set; } // Уникальный идентификатор автора
        public string FirstName { get; set; } // Имя автора
        public string LastName { get; set; } // Фамилия автора
        public DateTime BirthDate { get; set; } // Дата рождения автора
        public string Country { get; set; } // Страна происхождения автора

        public ICollection<Book> Books { get; set; } = []; // Коллекция книг, написанных автором (навигационное свойство)
    }
}
