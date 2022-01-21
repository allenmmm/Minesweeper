using System;
using System.Collections.Generic;

namespace Minesweeper.Interfaces
{
    public interface IMineManager
    {
        public List<Coordinate> Generate(int rows, int cols);
    }
}
