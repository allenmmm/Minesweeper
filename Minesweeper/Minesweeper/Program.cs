using System;

namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MineSweeperGame mineSweeper = null;
                do
                {
                    mineSweeper = new MineSweeperGame(
                        new GameController(
                            new ConsoleWrapper(),
                            new Board(8, 8,
                                new MineManager(
                                    new MineRandomiser())),
                            new Piece(0, 0, 2)));

                } while (mineSweeper.Run());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
