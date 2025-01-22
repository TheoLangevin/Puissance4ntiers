namespace Puissance4API.DTO;

public class AwaitingGameDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public PlayerDTO Host { get; set; } = new();
}