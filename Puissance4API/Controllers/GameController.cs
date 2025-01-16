using Microsoft.AspNetCore.Mvc;
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
                    Token = null
                }).ToList()
            }
        };

        _context.Games.Add(game);
        _context.SaveChanges();

        return Ok(game);
    }

    [HttpGet("waiting")]
    public IActionResult GetWaitingGames()
    {
        return Ok(_context.Games.Where(g => g.Status == "AwaitingGuest").ToList());
    }
}
