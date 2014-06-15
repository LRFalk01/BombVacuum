using System.Collections.Generic;

namespace BombVacuum.Models
{
    public class Board
    {
        public byte Rows { get; private set; }
        public byte Columns { get; private set; }
        public List<Square> Squares { get; private set; }
        public List<PlayerAction> PlayerActions { get; set; }
    }
}