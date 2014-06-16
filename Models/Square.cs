namespace BombVacuum.Models
{
    public class Square
    {
        public Square(byte column, byte row)
        {
            Column = column;
            Row = row;
            Status = SquareStatus.Unknown;
        }

        public byte Column { get; private set; }
        public byte Row { get; private set; }
        public SquareStatus Status { get; set; }
        public bool IsBomb { get; internal set; }
        public int NeighboringBombs { get; internal set; }
    }

    public enum SquareStatus
    {
        Unknown = 0,
        Flagged = 1,
        Revealed = 2
    }
}