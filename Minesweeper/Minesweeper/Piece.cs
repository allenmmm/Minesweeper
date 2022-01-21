using Minesweeper.Interfaces;

namespace Minesweeper
{
    public class Piece : Coordinate, IMovable
    {
        public int Lives { get; private set; }
        public virtual bool Alive { get { return Lives > -1; } }

        public Piece(
            int row,
            int col,
            int lives = 2) : base(row, col)
        {
            Lives = 
                Guard.AgainstLessThan(
                    0,
                    lives,
                    "Piece lives must be greater than 1");
        }

        public virtual void Move(
            string move,
            int maxRows,
            int maxCols)
        {
            var moveUC = move.ToUpper();
            if (moveUC == "U")
                Row = (Row + 1 >= maxRows ? 
                    Row : Row + 1);
            else if (moveUC == "D")
                Row = (Row - 1 < 0 ? 
                    Row : Row - 1);
            else if (moveUC == "R")
                Col = (Col + 1 >= maxCols ? 
                    Col : Col + 1);
            else if (moveUC == "L")
                Col = (Col - 1 < 0 ? 
                    Col : Col - 1);
        }

        public virtual void Hit()
        {
             Lives--;
        }

        public override string ToString()
        {
            var message = Lives == -1 ? $"Sorry all out of life!" : $"{Lives} lives remaining";
            return ($"Piece Status - {message}");
        }
    }
}
