using System.Text.Json.Serialization;

namespace Library_Web_application.Data.Entities
{
    /// <summary>
    /// Сущность: Автор книг
    /// </summary>
    public class Author
    {
        public int Id { get; set; } // ID автора
        public required string FirstName { get; set; } // Имя автора
        public required string LastName { get; set; } // Фамилия автора
        public required string Country { get; set; } // Страна происхождения автора
        public DateTime BirthDate { get; set; } // Дата рождения автора
        [JsonIgnore] public ICollection<Book> Books { get; set; } = []; // Коллекция книг автора (навигационное свойство)
    }
}
