namespace BombVacuum.Models
{
    public class Square
    {
        public byte Column { get; set; }
        public byte Row { get; set; }
        public SquareStatus Status { get; set; }
        public bool IsBomb { get; set; }
        public byte NeighboringBombs { get; set; }
    }

    public enum SquareStatus
    {
        Unknown,
        Flagged, 
        Safe,
        Bomb
    }
}