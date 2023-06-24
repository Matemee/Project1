using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Models;

namespace OnlineBookStoreAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Book>()
                .HasMany(bo => bo.Authors)
                .WithMany(a => a.Books);

            modelBuilder.Entity<Book>()
                .HasMany(bo => bo.Categories)
                .WithMany(c => c.Books);

            modelBuilder.Entity<Author>()
                .HasMany(au => au.Books)
                .WithMany(b => b.Authors);
        }

    }
}