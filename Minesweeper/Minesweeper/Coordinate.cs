namespace Minesweeper
{
    public class Coordinate 
    {
        public int Row { get; protected set; }
        public int Col { get; protected set; }

        public Coordinate(int row, int col)
        {
            Row = Guard.AgainstLessThan(0, row,"Row must be positive");
            Col = Guard.AgainstLessThan(0, col, "Col must be positive");
        }

        public Coordinate(Coordinate coordinate)
        {
            Row = coordinate.Row;
            Col = coordinate.Col;
        }

        public override bool Equals(object obj)
        {
            base.Equals(obj);
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Coordinate p = (Coordinate)obj;
                return (Row == p.Row &&
                    Col == p.Col);
            }
        }

        public override int GetHashCode()
        {
            return Row ^ Col;
        }
    }
}
