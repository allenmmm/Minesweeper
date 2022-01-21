namespace Minesweeper.Interfaces
{
    public interface IConsole
    {
        public void Write(string message);
        public void WriteLine(string message);
        public string ReadLine();
    }
}
