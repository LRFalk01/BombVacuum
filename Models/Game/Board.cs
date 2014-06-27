using System;
using System.Collections.Generic;
using System.Linq;
using BombVacuum.Helpers;

namespace BombVacuum.Models.Game
{
    public class Board
    {
        public Board(byte rows, byte columns, int bombs)
        {
            Rows = rows;
            Columns = columns;
            Bombs = bombs;
            BuildSqures();
        }

        private bool _boardInit;
        private readonly Random _random = RandomHelper.Instance.Random;
        private readonly object _lock = new object();

        public byte Rows { get; private set; }
        public byte Columns { get; private set; }
        public int Bombs { get; private set; }
        public int Flagged { get; internal set; }
        public List<Square> Squares { get; private set; }

        public Square Flag(byte row, byte column)
        {
            var square = Squares.FirstOrDefault(s => s.Row == row && s.Column == column);
            if (square == null) return null;
            switch (square.FlagStatus)
            {
                case FlagStatus.Unflagged:
                    square.FlagStatus = FlagStatus.Flagged;
                    break;
                case FlagStatus.Flagged:
                    square.FlagStatus = FlagStatus.Question;
                    break;
                default:
                    square.FlagStatus = FlagStatus.Unflagged;
                    break;
            }
            return square;
        }

        public List<Square> Reveal(byte row, byte column)
        {
            //only allow one user to init the board
            if (!_boardInit) InitBoard(column, row);
             
            var square = Squares.FirstOrDefault(s => s.Row == row && s.Column == column);
            if(square == null) return new List<Square>();
            lock (Squares)
            {
                if (square.State != SquareState.Unknown) return null;
                return RevealSquares(row, column, null);
            }
        }

        private List<Square> RevealSquares(byte row, byte column, List<Square> squares)
        {
            if(squares == null) squares = new List<Square>();
            var square = Squares.First(s => s.Row == row && s.Column == column);
            if (square.State != SquareState.Unknown || square.FlagStatus != FlagStatus.Unflagged) return squares;
            square.State = SquareState.Revealed;
            squares.Add(square);
            if (square.NeighboringBombs == 0 && !square.Bomb)
            {
                SquareNeighbors(square).ForEach(s => RevealSquares(s.Row, s.Column, squares));
            }
            return squares;
        }

        private void BuildSqures()
        {
            //build field
            Squares = new List<Square>();
            for (byte squareRow = 0; squareRow < Rows; squareRow++)
            {
                for (byte squareCol = 0; squareCol < Columns; squareCol++)
                {
                    Squares.Add(new Square(squareCol, squareRow));
                }
            }
        }

        private void InitBoard(byte startCol, byte startRow)
        {
            lock (_lock)
            {
                if (_boardInit) return;

                var numBombs = 0;
                while (numBombs < Bombs)
                {
                    var row = _random.Next(Rows);
                    var col = _random.Next(Columns);

                    //bomb cannot be first clicked square
                    if (startRow == row && startCol == col || Squares.First(x => x.Row == row && x.Column == col).Bomb) continue;
                    numBombs = numBombs + 1;
                    Squares.First(s => s.Row == row && s.Column == col).IsBomb = true;
                }

                Squares.AsParallel().ForAll(square =>
                {
                    square.NeighboringBombs = SquareNeighbors(square).Count(s => s.Bomb);
                });
                _boardInit = true;
            }
        }

        private List<Square> SquareNeighbors(Square square)
        {
            var rowLow = square.Row > 0 ? square.Row - 1 : 0;
            var rowHigh = square.Row < 255 ? square.Row + 1 : 0;
            var colLow = square.Column > 0 ? square.Column - 1 : 0;
            var colHigh = square.Column < 255 ? square.Column + 1 : 0;

            var squares = new List<Square>();
            squares.AddRange(
                Squares.Where(
                    s =>
                        s.Row >= rowLow && s.Row <= rowHigh && s.Column >= colLow && s.Column <= colHigh &&
                        s != square).ToList());
            return squares;
        } 
    }
}