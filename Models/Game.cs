﻿using System;
﻿using System.Collections.Generic;
﻿using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
﻿using BombVacuum.Helpers;

namespace BombVacuum.Models
{
    public class Game
    {
        public Game(byte columns, byte rows, int bombs)
        {
            Board = new Board(rows, columns, bombs);
            Id = Guid.NewGuid().ToString("D");
        }

        public List<Player> Players { get; set; }
        public Board Board { get; private set; }
        public string Id { get; private set; }
        public List<PlayerAction> PlayerActions { get; set; }


        public List<Square> Click(byte row, byte column)
        {
            return Board.Reveal(row, column);
        }

    }
}