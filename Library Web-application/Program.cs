using System.Text.Json.Serialization;
using Library_Web_application.Data.Context;
using Library_Web_application.Data.Repository;
using Library_Web_application.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            
            builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer
                (builder.Configuration.GetConnectionString("LibraryConnection")));

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
