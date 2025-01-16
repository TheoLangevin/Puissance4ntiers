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
        var existingPlayer = _context.Players.FirstOrDefault(p => p.Login == player.Login && p.Password == player.Password);
        if (existingPlayer == null) return Unauthorized("Invalid login or password.");

        return Ok(existingPlayer);
    }

    [HttpGet]
    public IActionResult GetAllPlayers()
    {
        return Ok(_context.Players.ToList());
    }
}
