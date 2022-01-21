using Minesweeper.Interfaces;
using System;

namespace Minesweeper
{
    public class MineRandomiser : IRandomise
    {
        private static Random rnd = new Random();

        public int GenerateInt(int upperLimit)
        {
            return rnd.Next(1,upperLimit+1);
        }

        public Coordinate GenerateCoordinate(int row, int col)
        {
            return (new Coordinate(rnd.Next(row), rnd.Next(col)));
        }
    }
}
