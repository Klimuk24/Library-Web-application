using System.ComponentModel.DataAnnotations;
using Library_Web_application.Data.Entities;

namespace Library.Tests;

public class AuthorModelTests
{
    [Theory]
    [InlineData("Лев", "Толстой", "Россия", true)]
    public void FirstName_Validation(string name, string lastName, string country, bool isValid)
    {
        var author = new Author 
        { 
            FirstName = name,
            LastName = lastName, 
            Country = country,
            BirthDate = DateTime.Now
        };

        var result = Validator.TryValidateObject(author, 
            new ValidationContext(author), 
            null, 
            true);

        Assert.Equal(isValid, result);
    }
}