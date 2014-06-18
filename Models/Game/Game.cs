using System;
using System.Collections.Generic;

namespace BombVacuum.Models.Game
{
    public class Game
    {
        public Game(byte columns, byte rows, int bombs)
        {
            Board = new Board(rows, columns, bombs);
            Id = Guid.NewGuid().ToString("D");
            Players = new List<Player>();
            PlayerActions = new List<PlayerAction>();
        }

        public List<Player> Players { get; private set; }
        public Board Board { get; private set; }
        public string Id { get; private set; }
        public List<PlayerAction> PlayerActions { get; private set; }


        public List<Square> Click(byte row, byte column)
        {
            return Board.Reveal(row, column);
        }

    }
}