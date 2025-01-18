using Puissance4Model.Models;

public class Cell
{
    public int Id { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public Token? Token { get; set; }

    // Nouvelle clé étrangère
    public int GridId { get; set; }

    public Cell() { }

    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
    }
}