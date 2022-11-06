using System.ComponentModel.DataAnnotations;
using ApiSeries.Validaciones;

namespace ApiSeries.Entidades
{
    public class Categoria
    {

        public int Id { get; set; }
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        [PrimerLetraMayuscula]
        public string Name { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public List<Tipos> Tipos { get; set; }

        public List<SerieCategoria> SerieCategoria { get; set; }
    }
}
