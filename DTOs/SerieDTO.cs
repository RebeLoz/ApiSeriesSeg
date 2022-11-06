using ApiSeries.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace ApiSeries.DTOs
{
    public class SerieDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [PrimerLetraMayuscula]
        public string Name { get; set; }
    }
}
