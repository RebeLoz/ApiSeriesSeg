using System.ComponentModel.DataAnnotations;
using ApiSeries.Validaciones;
namespace ApiSeries.DTOs
{
    public class CategoriaCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimerLetraMayuscula]
        public string Name { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<int> SeriesIds { get; set; }
    }
}
