using System;
using System.Collections.Generic;

namespace BombVacuum.Models
{
    public class Player
    {
        public Player(string name, string hash)
        {
            Name = name;
            Hash = hash;
        }
        public string ConnectionId { get; set; }
        public string Name { get; private set; }
        public string Hash { get; private set; }
        public string Group { get; set; }

        public bool IsPlaying { get; set; }
    }
}