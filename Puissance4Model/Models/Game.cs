using Puissance4Model.Models;
namespace Puissance4Model.Models;
public class Game
{
    public int Id { get; set; }
    public string Status { get; set; } 

    public int HostId { get; set; }
    public Player Host { get; set; }

    public int? GuestId { get; set; }
    public Player? Guest { get; set; }

    public Grid Grid { get; set; }
}