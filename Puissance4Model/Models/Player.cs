using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Puissance4Model.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        
        
        public ICollection<Game> GamesAsHost { get; set; }

        public ICollection<Game> GamesAsGuest { get; set; }
    
         public IEnumerable<Game> GetAllGames()
        {
            return GamesAsHost.Concat(GamesAsGuest);
        }

        /// Retourne les parties créées par le joueur mais qui attendent un invité.
        public IEnumerable<Game> GetAwaitingGames()
        {
            return GamesAsHost.Where(game => game.Status == GameStatus.AwaitingGuest.ToString());
        }

        /// Retourne les parties en cours dans lesquelles le joueur est impliqué.
        public IEnumerable<Game> GetInProgressGames()
        {
            return GetAllGames().Where(game => game.Status == GameStatus.InProgress.ToString());
        }

        /// Retourne les parties terminées auxquelles le joueur a participé.
        public IEnumerable<Game> GetFinishedGames()
        {
            return GetAllGames().Where(game => game.Status == GameStatus.Finished.ToString());
        }

        /// Vérifie si le joueur participe à une partie donnée.
        public bool IsPartOfGame(Game game)
        {
            return GamesAsHost.Contains(game) || GamesAsGuest.Contains(game);
        }

        /// Retourne le nombre total de parties jouées par le joueur.
        public int GetTotalGamesCount()
        {
            return GetAllGames().Count();
        }

        /// Vérifie si le joueur est actuellement impliqué dans une partie en cours.
        public bool IsInAnyOngoingGame()
        {
            return GetInProgressGames().Any();
        }

        /// Fournit une représentation textuelle du joueur.
        public override string ToString()
        {
            return $"Player [Id={Id}, Login={Login}, TotalGames={GetTotalGamesCount()}]";
        }
    }
}