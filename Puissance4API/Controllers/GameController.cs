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

    [HttpPost]
    public IActionResult CreateGame([FromBody] int hostId)
    {
        var host = _context.Players.Find(hostId);
        if (host == null) return BadRequest("Host not found.");

        var game = new Game
        {
            Host = host,
            Status = "AwaitingGuest",
            Grid = new Grid
            {
                Cells = Enumerable.Range(0, 6 * 7).Select(i => new Cell
                {
                    Row = i / 7,
                    Column = i % 7,
                }).ToList()
            }
        };

        _context.Games.Add(game);
        _context.SaveChanges();

        return Ok(game);
    }

    // Get a game by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGameById(int id)
    {
        var game = await _context.Games
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Status)
            .Include(g => g.Grid)
            .ThenInclude(grid => grid.Cells)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound(new { Message = "Game not found." });

        return Ok(game);
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
        var game = await _context.Games
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .FirstOrDefaultAsync(g => g.Id == request.GameId);

        if (game == null) return NotFound(new { Message = "Game not found." });
        if (game.Status != "AwaitingGuest") return BadRequest(new { Message = "Game is not open for joining." });

        var guest = await _context.Players.FindAsync(request.GuestId);
        if (guest == null) return BadRequest(new { Message = "Guest not found." });

        game.Guest = guest;
        game.Status = "InProgress";

        _context.Games.Update(game);
        await _context.SaveChangesAsync();

        return Ok(game);
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
            .FirstOrDefaultAsync(g => g.Id == request.GameId);

        if (game == null) return NotFound(new { Message = "Game not found." });

        var player = await _context.Players.FindAsync(request.PlayerId);
        if (player == null)
        {
            return BadRequest(new { Message = "Player not found." });
        }

        Token playerToken = new Token { Color = request.PlayerToken };

        var result = await PlayTurn(game, player, request.Column);

        return result;
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
        public int GuestId { get; set; }
    }
}
