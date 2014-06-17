using System;
using System.Collections.Generic;

namespace BombVacuum.Models
{
    public class Player
    {
        public Player(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }
        public string ConnectionId { get; set; }
        public string Name { get; private set; }
        public string Group { get; set; }

        public bool IsPlaying { get; set; }
        public string UserId { get; private set; }
    }
}