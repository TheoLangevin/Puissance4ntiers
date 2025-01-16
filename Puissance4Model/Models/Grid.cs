using System.ComponentModel.DataAnnotations.Schema;

namespace Puissance4Model.Models;

    public class Grid
    {
        public int Id { get; set; }
        public int Rows { get; set; } = 6;
        public int Columns { get; set; } = 7;

        public ICollection<Cell> Cells { get; set; }
    }
