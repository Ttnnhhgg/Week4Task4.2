using Microsoft.EntityFrameworkCore;
using Course4.Models;
namespace Course4.Data
{   
    public class LibraryContext : DbContext
    {        
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }              
        public DbSet<Author> Authors { get; set; }       
        public DbSet<Book> Books { get; set; }        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // НАСТРОЙКА СВЯЗИ ОДИН КО МНОГИМ           
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
            // НАСТРОЙКА ТАБЛИЦЫ AUTHOR
            modelBuilder.Entity<Author>(entity =>
            {                
                entity.HasKey(a => a.Id);                
                entity.Property(a => a.Name)
                    .IsRequired()
                    .HasMaxLength(100);             
                entity.Property(a => a.DateOfBirth)
                    .IsRequired();              
                entity.HasIndex(a => a.Name)
                    .HasDatabaseName("IX_Authors_Name");
            });
            // НАСТРОЙКА ТАБЛИЦЫ BOOK
            modelBuilder.Entity<Book>(entity =>
            {                
                entity.HasKey(b => b.Id);              
                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(200);               
                entity.Property(b => b.PublishedYear)
                    .IsRequired();               
                entity.Property(b => b.AuthorId)
                    .IsRequired();               
                entity.HasIndex(b => b.Title)
                    .HasDatabaseName("IX_Books_Title");              
                entity.HasIndex(b => b.PublishedYear)
                    .HasDatabaseName("IX_Books_PublishedYear");                
                entity.HasIndex(b => b.AuthorId)
                    .HasDatabaseName("IX_Books_AuthorId");
            });
            // начальные данные для базы
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "Лев Толстой",
                    DateOfBirth = new DateTime(1828, 9, 9)
                },
                new Author
                {
                    Id = 2,
                    Name = "Фёдор Достоевский",
                    DateOfBirth = new DateTime(1821, 11, 11)
                },
                new Author
                {
                    Id = 3,
                    Name = "Антон Чехов",
                    DateOfBirth = new DateTime(1860, 1, 29)
                }
            );
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Война и мир",
                    PublishedYear = 1869,
                    AuthorId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Анна Каренина",
                    PublishedYear = 1877,
                    AuthorId = 1
                },
                new Book
                {
                    Id = 3,
                    Title = "Преступление и наказание",
                    PublishedYear = 1866,
                    AuthorId = 2
                },
                new Book
                {
                    Id = 4,
                    Title = "Вишнёвый сад",
                    PublishedYear = 1904,
                    AuthorId = 3
                },
                new Book
                {
                    Id = 5,
                    Title = "Братья Карамазовы",
                    PublishedYear = 1880,
                    AuthorId = 2
                }
            );
        }
    }
}