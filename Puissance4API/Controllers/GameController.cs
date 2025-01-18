using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puissance4Model.Data;
using Puissance4Model.Models;

namespace Puissance4API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GamesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    [Authorize]
    public IActionResult CreateGame()
    {
        var hostIdClaim = User.FindFirst("PlayerId")?.Value;
        if (string.IsNullOrEmpty(hostIdClaim) || !int.TryParse(hostIdClaim, out int hostId) || hostId <= 0)
        {
            return Unauthorized(new { Message = "HostId invalide ou non trouvé dans le token." });
        }

        var host = _context.Players.FirstOrDefault(p => p.Id == hostId);
        if (host == null)
        {
            return NotFound(new { Message = "Host not found." });
        }

        // Création de la partie
        var game = new Game
        {
            Host = host,
            Status = GameStatus.AwaitingGuest.ToString(),
            Grid = new Grid()
        };
        game.Grid.InitializeCells();

        _context.Games.Add(game);
        _context.SaveChanges();

        // Retourner uniquement l'ID de la partie
        return Ok(game.Id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGameById(int id)
    {
        var game = await _context.Games
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Grid)
            .ThenInclude(grid => grid.Cells)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound(new { Message = "Game not found." });
        }

        // Projeter un modèle simplifié
        var gameDto = new
        {
            game.Id,
            game.Status,
            Host = new { game.Host.Id, game.Host.Login },
            Guest = game.Guest == null ? null : new { game.Guest.Id, game.Guest.Login },
            Grid = new
            {
                game.Grid.Rows,
                game.Grid.Columns,
                Cells = game.Grid.Cells.Select(c => new { c.Row, c.Column, c.Token?.Color })
            }
        };

        return Ok(gameDto);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllGames()
    {
        var awaitingGames = await _context.Games
            .Where(g => g.Status == GameStatus.AwaitingGuest.ToString())
            .Include(g => g.Host)
            .ToListAsync();

        var inProgressGames = await _context.Games
            .Where(g => g.Status == GameStatus.InProgress.ToString())
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .ToListAsync();

        // Projeter un modèle simplifié
        var result = new
        {
            AwaitingGuest = awaitingGames.Select(game => new
            {
                game.Id,
                game.Status,
                Host = new { game.Host.Id, game.Host.Login }
            }),
            InProgress = inProgressGames.Select(game => new
            {
                game.Id,
                game.Status,
                Host = new { game.Host.Id, game.Host.Login },
                Guest = game.Guest == null ? null : new { game.Guest.Id, game.Guest.Login }
            })
        };

        return Ok(result);
    }

    // Get all games waiting for a guest
    [HttpGet("waiting")]
    public async Task<IActionResult> GetWaitingGames()
    {
        var games = await _context.Games
            .Where(g => g.Status == "AwaitingGuest")
            .Include(g => g.Host)
            .ToListAsync();

        return Ok(games);
    }

    // Join a game
    [HttpPost("join")]
    public async Task<IActionResult> JoinGame([FromBody] JoinGameRequest request)
    {
        try
        {
            // Extraire l'ID du joueur (GuestId) depuis le JWT
            var guestIdClaim = User.FindFirst("PlayerId")?.Value;

            if (string.IsNullOrEmpty(guestIdClaim) || !int.TryParse(guestIdClaim, out int guestId) || guestId <= 0)
            {
                Console.WriteLine("Failed to extract PlayerId from JWT.");
                return Unauthorized(new { Message = "Invalid guest ID." });
            }

            Console.WriteLine($"Extracted PlayerId from JWT: {guestId}");

            var game = await _context.Games
                .Include(g => g.Host)
                .Include(g => g.Guest)
                .FirstOrDefaultAsync(g => g.Id == request.GameId);

            if (game == null)
            {
                Console.WriteLine($"Game with ID {request.GameId} not found.");
                return NotFound(new { Message = "Game not found." });
            }

            if (game.Status != GameStatus.AwaitingGuest.ToString())
            {
                Console.WriteLine($"Game with ID {request.GameId} is not open for joining.");
                return BadRequest(new { Message = "Game is not open for joining." });
            }

            var guest = await _context.Players.FindAsync(guestId);
            if (guest == null)
            {
                Console.WriteLine($"Guest with ID {guestId} not found in database.");
                return BadRequest(new { Message = "Guest not found." });
            }

            // Vérifier si le joueur est déjà engagé dans une autre partie
            var existingGame = _context.Games.FirstOrDefault(g =>
                (g.HostId == guest.Id || g.GuestId == guest.Id) && g.Status == GameStatus.InProgress.ToString());
            if (existingGame != null)
            {
                Console.WriteLine($"Guest with ID {guestId} is already in another game.");
                return BadRequest(new { Message = "Guest is already in another game." });
            }

            game.Guest = guest;
            game.Status = "InProgress";

            _context.Games.Update(game);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Guest with ID {guestId} successfully joined game ID {game.Id}.");
            return Ok(new { Message = "You have joined the game!", GameId = game.Id });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in JoinGame: {ex.Message}");
            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }
    }

    private async Task<IActionResult> PlayTurn(Game game, Player player, int column)
    {
        // Vérifier si c'est bien à ce joueur de jouer
        if (game.Status != "InProgress")
        {
            return BadRequest(new { Message = "Game is not in progress." });
        }

        // Déterminer le jeton à utiliser pour ce joueur
        Token token = player == game.Host ? new Token { Color = "Red" } : new Token { Color = "Yellow" };

        // Effectuer le coup
        bool success = game.Grid.DropToken(column, token);
        if (!success)
        {
            return BadRequest(new { Message = "Column is full." });
        }

        // Vérifier la condition de victoire
        if (game.Grid.CheckWinCondition(token))
        {
            game.Status = "Finished";
            await _context.SaveChangesAsync();
            return Ok(new { Message = $"{player.Login} wins!" });
        }

        // Vérifier si la grille est pleine (match nul)
        if (game.Grid.IsFull())
        {
            game.Status = "Finished";
            await _context.SaveChangesAsync();
            return Ok(new { Message = "It's a draw!" });
        }

        // Passer au tour suivant (changer le joueur actif)
        game.Status = player == game.Host ? "Guest's Turn" : "Host's Turn";
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Turn played successfully." });

    }

    [HttpPost("play")]
    public async Task<IActionResult> PlayTurn([FromBody] PlayTurnRequest request)
    {
        var game = await _context.Games
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Grid)
            .ThenInclude(grid => grid.Cells)
            .FirstOrDefaultAsync(g => g.Id == request.GameId);

        if (game == null) return NotFound(new { Message = "Game not found." });

        var player = await _context.Players.FindAsync(request.PlayerId);
        if (player == null)
        {
            return BadRequest(new { Message = "Player not found." });
        }

        // Vérifiez si le joueur est autorisé à jouer
        if (player.Id != game.HostId && player.Id != game.GuestId)
        {
            return BadRequest(new { Message = "Player is not part of this game." });
        }

        try
        {
            var result = await PlayTurn(game, player, request.Column);
            return result;
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }



    public class PlayTurnRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int Column { get; set; }
        public string PlayerToken { get; set; }
    }



    // Request model for joining a game
    public class JoinGameRequest
    {
        public int GameId { get; set; }
    }
}
