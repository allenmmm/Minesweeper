using Minesweeper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper
{
    public class MineManager : IMineManager
    {
        private static Random rnd = new Random();
        public IRandomise RandomMineGenerator { get; private set; }

        public MineManager(
            IRandomise radomiser)
        {
            RandomMineGenerator = Guard.AgainstNull(
                radomiser,
                "Game configuration object must not be null");
        }

        public virtual List<Coordinate> Generate(int rows, int cols)
        {
            var numberOfMinesToPrime = RandomMineGenerator.GenerateInt(rows * cols);
            var minesToPrime = new List<Coordinate>();
            while (minesToPrime.Count < numberOfMinesToPrime)
            {
                Coordinate mine = new Coordinate(
                    RandomMineGenerator.GenerateCoordinate(rows, cols));

                if (minesToPrime.Where(p => p.Equals(mine)).Count()==0)
                {
                    minesToPrime.Add(mine);
                }
            }
            return (minesToPrime);
        }
    }
}
