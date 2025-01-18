using System.ComponentModel.DataAnnotations.Schema;

namespace Puissance4Model.Models;

public class Grid
{
    public int Id { get; set; }
    public int Rows { get; set; } = 6;
    public int Columns { get; set; } = 7;

    // Clé étrangère vers Game
    public int GameId { get; set; }

    public ICollection<Cell> Cells { get; set; } = new List<Cell>();

    public void InitializeCells()
    {
        Cells.Clear(); // Réinitialiser les cellules

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                Cells.Add(new Cell
                {
                    Row = row,
                    Column = column,
                    GridId = this.Id // Associer la cellule à la grille actuelle
                });
            }
        }
    }

    // Ajoute un jeton dans une colonne
    public bool DropToken(int column, Token token)
    {
        if (column < 0 || column >= Columns)
            throw new ArgumentException("Colonne invalide.");

        // Trouver la première cellule vide dans la colonne (de bas en haut)
        var cell = Cells.Where(c => c.Column == column && c.Token == null)
                        .OrderByDescending(c => c.Row)
                        .FirstOrDefault();

        if (cell != null)
        {
            cell.Token = token;
            return true; // Jeton placé avec succès
        }

        return false; // Colonne pleine
    }

    // Vérifie si la grille est complètement remplie
    public bool IsFull()
    {
        return Cells.All(c => c.Token != null);
    }

    // Vérifie si un joueur a gagné
    public bool CheckWinCondition(Token token)
    {
        // Vérifie les alignements (horizontal, vertical, diagonal)
        return CheckHorizontal(token) || CheckVertical(token) || CheckDiagonals(token);
    }

    private bool CheckHorizontal(Token token)
    {
        foreach (var row in Enumerable.Range(0, Rows))
        {
            int count = 0;
            foreach (var cell in Cells.Where(c => c.Row == row).OrderBy(c => c.Column))
            {
                count = (cell.Token == token) ? count + 1 : 0;
                if (count >= 4) return true;
            }
        }
        return false;
    }

    private bool CheckVertical(Token token)
    {
        foreach (var column in Enumerable.Range(0, Columns))
        {
            int count = 0;
            foreach (var cell in Cells.Where(c => c.Column == column).OrderBy(c => c.Row))
            {
                count = (cell.Token == token) ? count + 1 : 0;
                if (count >= 4) return true;
            }
        }
        return false;
    }

    private bool CheckDiagonals(Token token)
    {
        // Diagonales montantes
        foreach (var cell in Cells)
        {
            if (CheckDirection(cell, token, 1, 1) || CheckDirection(cell, token, 1, -1))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckDirection(Cell startCell, Token token, int rowStep, int colStep)
    {
        int count = 0;
        int row = startCell.Row;
        int column = startCell.Column;

        while (row >= 0 && row < Rows && column >= 0 && column < Columns)
        {
            var cell = Cells.FirstOrDefault(c => c.Row == row && c.Column == column);
            if (cell?.Token == token)
            {
                count++;
                if (count >= 4) return true;
            }
            else
            {
                break;
            }

            row += rowStep;
            column += colStep;
        }

        return false;
    }

    // Affiche la grille (pour debug)
    public void PrintGrid()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                var cell = Cells.FirstOrDefault(c => c.Row == row && c.Column == column);
                Console.Write(cell?.Token?.Color ?? ".");
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
