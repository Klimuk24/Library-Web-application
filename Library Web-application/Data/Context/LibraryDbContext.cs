using Library_Web_application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_application.Data.Context
{
    public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурации Fluent API
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.HasIndex(b => b.ISBN).IsUnique();
                entity.Property(b => b.Title).IsRequired().HasMaxLength(128);
                entity.Property(b => b.ISBN).IsRequired().HasMaxLength(17); // ISBN-13 

                entity.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(32);
                entity.Property(a => a.LastName).IsRequired().HasMaxLength(32);    
                entity.Property(a => a.Country).IsRequired().HasMaxLength(64);
            });
        }
    }
}
