using BombVacuum.Models.Game;

namespace BombVacuum.Models.DTO
{
    public class SquareDTO
    {
        public SquareStatus Status { get; set; }
        public byte Row { get; set; }
        public byte Column { get; set; }
        public bool? Bomb { get; set; }
        public int? NeighboringBombs { get; set; }
    }
}