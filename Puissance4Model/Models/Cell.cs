namespace Puissance4Model.Models;


    public class Cell
    {
        
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Token Token { get; set; }

        public Cell() { }

        public Cell(int Row, int Column)
        {
            this.Row = Row;
            this.Column = Column;
            Token = null;
        }
    }

