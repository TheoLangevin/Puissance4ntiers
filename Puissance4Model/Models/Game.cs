using System.Drawing;
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

    // Méthode pour rejoindre la partie
        public void JoinGame(Player guest)
        {
            if (Guest != null)
            {
                throw new InvalidOperationException("La partie a déjà un invité.");
            }

            Guest = guest;
            GuestId = guest.Id;
            Status = GameStatus.InProgress.ToString();
        }

        // Méthode pour jouer un tour
        public void PlayTurn(Player player, int column)
        {
            if (Status != GameStatus.InProgress.ToString())
            {
                throw new InvalidOperationException("La partie n'est pas en cours.");
            }

            if (player.Id != HostId && player.Id != GuestId)
            {
                throw new UnauthorizedAccessException("Seuls l'hôte ou l'invité peuvent jouer dans cette partie.");
            }

            Token playerToken = GetPlayerToken(player);

            if (!Grid.DropToken(column, playerToken))
            {
                throw new InvalidOperationException("La colonne est pleine ou invalide.");
            }

            if (Grid.CheckWinCondition(playerToken))
            {
                Status = GameStatus.Finished.ToString();
            }
            else if (Grid.IsFull())
            {
                Status = GameStatus.Finished.ToString();
            }
        }

        // Obtient le jeton associé à un joueur
        private Token GetPlayerToken(Player player)
        {
            if (player.Id == HostId)
                return new Token{Color = "Red"}; // Exemple : l'hôte a un jeton rouge
            else if (player.Id == GuestId)
                return new Token{Color = "Yellow"}; // Exemple : l'invité a un jeton jaune

            throw new InvalidOperationException("Joueur non associé à cette partie.");
        }

        // Redéfinition de ToString pour des logs ou affichages
        public override string ToString()
        {
            return $"Game [Id={Id}, Status={Status}, Host={Host?.Login}, Guest={Guest?.Login}]";
        }
    }

// Enum pour le statut de la partie
public enum GameStatus
{
    AwaitingGuest,
    InProgress,
    Finished
}