public class CellDto
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string? TokenColor { get; set; } // La couleur du jeton, ou null si la cellule est vide
}