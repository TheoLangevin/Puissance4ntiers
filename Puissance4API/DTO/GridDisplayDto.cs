using Puissance4API.DTO;

public class GridDisplayDto
{
    public List<CellDto> Cells { get; set; } = new();
    public int Rows { get; set; } = 6;
    public int Columns { get; set; } = 7;

}
