using Minesweeper.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper
{
    public class Board
    {
        public readonly int Rows;
        public readonly int Cols;
        public readonly int NumberOfMines;
        private readonly List<Coordinate> _Mines = new List<Coordinate>();
        public IEnumerable<Coordinate> Mines => _Mines.AsReadOnly();

        public Board(int rows, int cols, IMineManager mineManager)
        {
            Rows = Guard.AgainstLessThan(1, rows, "Rows must be greater than zero");
            Cols = Guard.AgainstLessThan(1, cols, "Rows must be greater than zero");
            _Mines = mineManager.Generate(Rows, Cols);
            NumberOfMines = _Mines.Count;
        }

        public virtual bool IsMine(Coordinate source)
        {
            var mine = Mines.FirstOrDefault(m => m.Equals(new Coordinate(source)));
            if (mine != null)
            {
                _Mines.Remove(mine);
                return true;
            }
            return false;
        }

        public bool IsComplete(Coordinate source)
        {
            return (source.Row == Rows - 1 ? true : false);
        }

        public override string ToString()
        {
            var s = $"Board Status - {_Mines.Count}/{NumberOfMines} mines remaining";
            return s;
        }
    }
}
