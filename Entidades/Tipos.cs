using Microsoft.AspNetCore.Identity;

namespace ApiSeries.Entidades
{
    public class Tipos
    {
        public int Id { get; set; }
        public string Contenido { get; set; }

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario { get; set; }
    }
}
