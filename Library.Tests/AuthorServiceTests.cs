using Library_Web_application.Data.Entities;
using Library_Web_application.Data.Repository.Interfaces;
using Library_Web_application.Services;
using Moq;

namespace Library.Tests;

public class AuthorServiceTests
{
    private readonly Mock<IAuthorRepository> _mockRepo;
    private readonly AuthorService _service;

    public AuthorServiceTests()
    {
        _mockRepo = new Mock<IAuthorRepository>();
        _service = new AuthorService(_mockRepo.Object);
    }

    [Fact]
    public void GetById_CallsRepository_Once()
    {
        var testAuthor = new Author 
        {
            Id = 1,
            FirstName = "Test",      
            LastName = "Author",     
            Country = "Country",     
            BirthDate = DateTime.Now 
        };

        _mockRepo.Setup(r => r.GetById(1))
            .Returns(testAuthor);
        
        var result = _service.GetById(1);
        
        _mockRepo.Verify(r => r.GetById(1), Times.Once);
        Assert.Equal(testAuthor, result);
    }
}