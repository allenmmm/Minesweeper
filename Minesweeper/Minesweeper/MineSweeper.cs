using Minesweeper.Interfaces;

namespace Minesweeper
{
    public class MineSweeperGame
    {
        private readonly IGameController _GameController;

        public MineSweeperGame(IGameController gameController)
        {
            _GameController = gameController;
        }

        public bool Run()
        {
            _GameController.DisplayGameSummary(
                _GameController.Play());
            return _GameController.PlayAgain();
        }
    }
}
