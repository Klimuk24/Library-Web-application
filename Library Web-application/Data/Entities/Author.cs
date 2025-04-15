using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library_Web_application.Data.Entities
{
    /// <summary>
    /// Сущность: Автор книг
    /// </summary>
    public class Author
    {
        public int Id { get; set; } // ID автора
        public DateTime BirthDate { get; set; } // Дата рождения автора
        
        [Required]
        public string FirstName { get; set; } // Имя автора
        public string LastName { get; set; } // Фамилия автора
        public string Country { get; set; } // Страна происхождения автора
        
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = []; // Коллекция книг автора (навигационное свойство)
    }
}
