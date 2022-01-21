using Minesweeper.Interfaces;

namespace Minesweeper
{
    public class GameController : IGameController
    {
        private readonly IConsole _Console;
        private readonly Board _Board;
        private readonly Piece _Piece;
        public  GameController(
            IConsole console,
            Board board,
            Piece piece)
        {
            _Console = console;
            _Board = board;
            _Piece = piece;
            _Console.WriteLine("=========================================================");
            _Console.WriteLine("");
            _Console.WriteLine("                WELCOME TO MINESWEEPER");
            _Console.WriteLine("");
            _Console.WriteLine("Developed by : Matt Allen");
            _Console.WriteLine("=========================================================");
            _Console.WriteLine($"Try your luck in this ({board.Rows},{board.Cols}) grid of madness");
        }

        public virtual bool PlayAgain()
        {
            _Console.Write("Do you wish to continue Y/N : ");
            var response = _Console.ReadLine().ToUpper();
            _Console.WriteLine("");
            _Console.WriteLine("");
            _Console.WriteLine("");
            return response == "Y" ? true : false;
        }

        public virtual void  DisplayGameSummary(bool succeeded)
        {
            _Console.WriteLine("");
            _Console.WriteLine("///////////////////////////////////////////////////////////");
            if (succeeded)
                _Console.WriteLine("      CONGRATULATIONS YOU WON");
            else
                _Console.WriteLine("                   UNLUCKY YOU LOST THIS TIME");
            
            _Console.WriteLine("///////////////////////////////////////////////////////////");
            _Console.WriteLine("Reivew your game summary below:");
            _Console.WriteLine($"\t{_Piece}");
            _Console.WriteLine($"\t{_Board}");
            _Console.WriteLine("///////////////////////////////////////////////////////////");
            _Console.WriteLine("");
        }

        public virtual bool Play()
        {
            do
            {
                _Console.WriteLine("");
                _Console.Write($"ENTER MOVE ({_Piece.Row}/{_Piece.Col}) : ");
 
                 _Piece.Move(
                    _Console.ReadLine(),
                    _Board.Rows,
                    _Board.Cols);
   

                if (_Board.IsMine(_Piece))
                {
                    _Piece.Hit();
                    _Console.WriteLine("");
                    _Console.WriteLine("          ****************** BOOM! ****************          ");
                }
            } while (_Piece.Alive && !_Board.IsComplete(_Piece));
            return (_Piece.Alive);
        }
    }
}
