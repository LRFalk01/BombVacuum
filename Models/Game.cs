using System.Collections.Generic;

namespace BombVacuum.Models
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public Board Board { get; set; }
    }
}