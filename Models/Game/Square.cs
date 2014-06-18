namespace BombVacuum.Models.Game
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

        internal bool Bomb;
        public bool? IsBomb
        {
            get
            {
                if (Status == SquareStatus.Unknown) return null;
                return Bomb;
            }
            internal set { Bomb = value ?? false; }
            
        }
        public int NeighboringBombs { get; internal set; }
    }

    public enum SquareStatus
    {
        Unknown = 0,
        Flagged = 1,
        Revealed = 2
    }
}