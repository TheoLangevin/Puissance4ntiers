using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Puissance4Model.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        
        
        public ICollection<Game> GamesAsHost { get; set; }

        public ICollection<Game> GamesAsGuest { get; set; }
    }
}