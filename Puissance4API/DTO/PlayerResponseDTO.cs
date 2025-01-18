using System.ComponentModel.DataAnnotations;

namespace Puissance4API.DTO
{
    public class PlayerResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
    }
}