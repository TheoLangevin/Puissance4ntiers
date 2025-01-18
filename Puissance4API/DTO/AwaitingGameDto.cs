public class AwaitingGameDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public HostDto Host { get; set; } = new();
}