public class InProgressGameDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public HostDto Host { get; set; } = new();
    public GuestDto? Guest { get; set; }
}