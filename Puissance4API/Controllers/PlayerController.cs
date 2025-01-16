using Microsoft.AspNetCore.Mvc;
using Puissance4Model.Data;
using Puissance4Model.Models;

namespace Puissance4API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PlayersController(ApplicationDbContext context)
    {
        _context = context;
    }
    

    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] Player player)
    {
        if (player == null || string.IsNullOrEmpty(player.Login) || string.IsNullOrEmpty(player.Password))
        {
            return BadRequest("Login and password are required.");
        }

        var existingPlayer = _context.Players.FirstOrDefault(p => p.Login == player.Login && p.Password == player.Password);
        if (existingPlayer == null)
        {
            return Unauthorized("Invalid login or password.");
        }

        // For future extensibility: Add a token or session management
        return Ok(new
        {
            Id = existingPlayer.Id,
            Login = existingPlayer.Login,
            Message = "Authentication successful."
        });
    }

    // Endpoint to get all players
    [HttpGet]
    public IActionResult GetAllPlayers()
    {
        var players = _context.Players.Select(p => new
        {
            p.Id,
            p.Login
        }).ToList();

        return Ok(players);
    }

    // Endpoint to retrieve a player by ID
    [HttpGet("{id}")]
    public IActionResult GetPlayerById(int id)
    {
        var player = _context.Players.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            return NotFound("Player not found.");
        }

        return Ok(new
        {
            player.Id,
            player.Login
        });
    }

}
