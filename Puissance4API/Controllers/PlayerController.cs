using Microsoft.AspNetCore.Mvc;
using Puissance4Model.Data;
using Puissance4Model.Models;
using Puissance4API.DTO;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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

    [HttpPost("register")]
    public async Task<IActionResult> Register(PlayerDTO playerDto)
    {
        if (_context.Players.Any(p => p.Login == playerDto.Login))
        {
            return Conflict("Login already exists.");
        }

        var player = new Player
        {
            Login = playerDto.Login,
            Password = BCrypt.Net.BCrypt.HashPassword(playerDto.Password)
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayer(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        return Ok(player);
    }

    [HttpGet]
    public IActionResult GetPlayers()
    {
        return Ok(_context.Players.ToList());
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] PlayerDTO loginData)
    {
        var player = _context.Players.SingleOrDefault(p => p.Login == loginData.Login);
        if (player == null || !BCrypt.Net.BCrypt.Verify(loginData.Password, player.Password))
        {
            return Unauthorized(new { Message = "Invalid login or password." });
        }

        // Générer un token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("MotDePasseVraimentCacher"); // Clé secrète pour signer le token

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, player.Login),
            new Claim("PlayerId", player.Id.ToString())
        }),
            Expires = DateTime.UtcNow.AddHours(1), // Durée de validité du token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new
        {
            Token = tokenHandler.WriteToken(token),
            Player = new PlayerResponseDTO
            {
                Id = player.Id,
                Login = player.Login
            }
        });
    }
}
