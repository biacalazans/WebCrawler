using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;

namespace WebCrawler
{
    public class WebCrawlerContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }

        public DbSet<Autor> Autores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=WebCrawler;user=root;password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.AutorId);
               
            });

            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.LivroId);
                entity.HasOne(d => d.Autor)
                  .WithMany(p => p.Livros);
                
            });
        }

    }
}
