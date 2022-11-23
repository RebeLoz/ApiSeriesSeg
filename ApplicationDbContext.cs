using ApiSeries.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiSeries
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SerieCategoria>()
                .HasKey(al => new { al.SerieId, al.CategoriaId });
        }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Tipos> Tipos { get; set; }
        public DbSet<SerieCategoria> SerieCategoria { get; set; }
    }
}
