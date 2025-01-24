using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puissance4Model.Data;
using Puissance4Model.Models;
using Puissance4API.DTO;

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

        // Projeter les données dans le DTO
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
                Cells = game.Grid.Cells.Select(c => new { c.Row, c.Column, c.Token })
            }
        };

        return Ok(gameDto);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllGames()
    {
        var awaitingGames = await _context.Games
            .Where(g => g.Status == "AwaitingGuest")
            .Include(g => g.Host)
            .ToListAsync();

        var inProgressGames = await _context.Games
            .Where(g => g.Status == "InProgress")
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

            if (game.Status != "AwaitingGuest")
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

            if (game.HostId == guest.Id)
            {
                return Ok(new { Message = "You rejoined the game!", GameId = game.Id });
            }

            //Vérifier si le joueur est déjà engagé dans une autre partie
            // var existingGame = _context.Games.FirstOrDefault(g =>
            //     (g.HostId == guest.Id || g.GuestId == guest.Id) && g.Status == GameStatus.InProgress.ToString());
            // if (existingGame != null)
            // {
            //     Console.WriteLine($"Guest with ID {guestId} is already in another game.");
            //     return BadRequest(new { Message = "Guest is already in another game." });
            // }



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


    [HttpPost("{id}/play")]
    [Authorize]
    public async Task<IActionResult> PlayTurn(int id, [FromBody] PlayTurnRequest request)
    {
        var playerIdClaim = User.FindFirst("PlayerId")?.Value;
        if (string.IsNullOrEmpty(playerIdClaim) || !int.TryParse(playerIdClaim, out int playerId))
        {
            return Unauthorized(new { Message = "Invalid player ID." });
        }

        Console.WriteLine($"Extracted PlayerId from JWT: {playerId}");

        var game = await _context.Games
            .Include(g => g.Grid)
            .ThenInclude(grid => grid.Cells)
            .Include(game => game.Host)
            .Include(game => game.Guest)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound(new { Message = "Game not found." });
        }


        // Vérification de l'état et du joueur
        if (game.Status == "Finished" || game.Status == "AwaitingGuest" ||
            (game.Host.Id != playerId && game.Guest?.Id != playerId))
        {
            return BadRequest(new { Message = "Invalid turn or unauthorized player." });
        }

        var token = game.Host.Id == playerId ? "Red" : "Yellow";

        if (!game.Grid.DropToken(request.Column, token))
        {
            return BadRequest(new { Message = "Column is full." });
        }

        // Vérifier la victoire ou égalité
        if (game.Grid.CheckWinCondition(token))
        {
            game.Status = "Finished";
        }
        else if (game.Grid.IsFull())
        {
            game.Status = "Finished";
        }
        else
        {
            // Passage de tour
            game.Status = game.Status == "Host's Turn" ? "Guest's Turn" : "Host's Turn";
        }

        _context.Games.Update(game); // Met à jour la partie
        var changes = await _context.SaveChangesAsync();
        Console.WriteLine($"Number of changes saved to the database: {changes}");

        return Ok(new { Message = "Turn played successfully." });
    }




    public class PlayTurnRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int Column { get; set; }
    }



    // Request model for joining a game
    public class JoinGameRequest
    {
        public int GameId { get; set; }
    }

}
