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
            Id = Guid.NewGuid().ToString("D");
        }
        public string ConnectionId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Group { get; set; }

        public bool IsPlaying { get; set; }
    }
}