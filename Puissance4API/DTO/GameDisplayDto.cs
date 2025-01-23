using System.ComponentModel.DataAnnotations;
using Puissance4API.DTO;

public class GameDisplayDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public PlayerResponseDTO Host { get; set; }
    public PlayerResponseDTO? Guest { get; set; }
    public GridDisplayDto Grid { get; set; }
}