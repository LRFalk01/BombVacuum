using System;

namespace BombVacuum.Models.Game
{
    public class Square
    {
        public Square(byte column, byte row)
        {
            Column = column;
            Row = row;
            State = SquareState.Unknown;
        }

        public byte Column { get; private set; }
        public byte Row { get; private set; }
        public SquareState State { get; set; }
        public FlagStatus FlagStatus { get; set; }

        internal bool Bomb;
        public bool? IsBomb
        {
            get
            {
                if (State == SquareState.Unknown) return null;
                return Bomb;
            }
            internal set { Bomb = value ?? false; }
            
        }
        public int NeighboringBombs { get; internal set; }
    }

    public enum SquareState
    {
        Unknown = 0,
        Revealed = 1
    }

    public enum FlagStatus
    {
        Unflagged = 0,
        Flagged = 1,
        Question = 2
    }
}