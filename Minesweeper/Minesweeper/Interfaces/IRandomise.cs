namespace Minesweeper.Interfaces
{
    public interface IRandomise
    {
        Coordinate GenerateCoordinate(
            int row,
            int col);

        int GenerateInt(int upperLimit);
    }
}
