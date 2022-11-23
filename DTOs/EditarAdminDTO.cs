using System.ComponentModel.DataAnnotations;

namespace ApiSeries.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
