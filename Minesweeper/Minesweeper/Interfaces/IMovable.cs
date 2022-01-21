

namespace Minesweeper.Interfaces
{
    public interface IMovable
    {
        public  void Move(
            string move,
            int maxRows,
            int maxCols);

        public  void Hit();
    }
}
