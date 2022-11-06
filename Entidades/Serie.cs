using System.ComponentModel.DataAnnotations;
using ApiSeries.Validaciones;

namespace ApiSeries.Entidades
{
    public class Serie
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        [PrimerLetraMayuscula]
        public string Name { get; set; }
        public List<SerieCategoria> SerieCategoria { get; set; }
    }
}
