

using System.ComponentModel.DataAnnotations;

namespace Puissance4API.DTO
{
    public class PlayerDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}