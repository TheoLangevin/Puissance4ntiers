using Puissance4API.DTO;

public class GamesDto
{
    public List<AwaitingGameDto> AwaitingGuest { get; set; } = new();
    public List<InProgressGameDto> InProgress { get; set; } = new();
}