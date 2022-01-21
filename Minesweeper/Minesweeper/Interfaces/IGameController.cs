namespace Minesweeper.Interfaces
{
    public interface IGameController
    {
        public bool Play();
        public void DisplayGameSummary(bool succeeeded);
        public bool PlayAgain();
    }
}
