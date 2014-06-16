using System;
using System.Collections.Generic;
using System.Linq;
using BombVacuum.Helpers;

namespace BombVacuum.Models
{
    public class Board
    {
        public Board(byte rows, byte columns, int bombs)
        {
            Rows = rows;
            Columns = columns;
            Bombs = bombs;
        }

        private bool _boardInit;
        private readonly Random _random = RandomHelper.Instance.Random;
        private readonly object _lock = new object();

        public byte Rows { get; private set; }
        public byte Columns { get; private set; }
        public int Bombs { get; private set; }
        public List<Square> Squares { get; private set; }

        public List<Square> Reveal(byte row, byte column)
        {
            //only allow one user to init the board
            if (!_boardInit)
            {
                lock (_lock)
                {
                    if (!_boardInit) InitBoard(column, row);
                }   
            }
            var square = Squares.First(s => s.Row == row && s.Column == column);
            lock (Squares)
            {
                if (square.Status != SquareStatus.Unknown) return null;
                return RevealSquares(row, column);
            }
        }

        private List<Square> RevealSquares(byte row, byte column)
        {
            var squaresRevealed = new List<Square>();
            var square = Squares.First(s => s.Row == row && s.Column == column);
            if (square.Status != SquareStatus.Unknown) return squaresRevealed;
            squaresRevealed.Add(square);
            if (square.NeighboringBombs == 0)
            {
                SquareNeighbors(square).ForEach(s => squaresRevealed.AddRange(RevealSquares(s.Row, s.Column)));
            }
            return squaresRevealed;
        } 

        private void InitBoard(byte startCol, byte startRow)
        {
            if (_boardInit) return;

            //build field
            Squares = new List<Square>();
            for (byte squareRow = 0; squareRow < Rows; squareRow++)
            {
                for (byte squareCol = 0; squareCol < Columns; squareCol++)
                {
                    Squares.Add(new Square(squareCol, startRow));
                }
            }
            var numBombs = 0;
            while (numBombs < Bombs)
            {
                var row = _random.Next(Rows - 1);
                var col = _random.Next(Columns - 1);

                //bomb cannot be first clicked square
                if(startRow == row && startCol == col) continue;
                numBombs = numBombs + 1;
                Squares.First(s => s.Row == row && s.Column == col).IsBomb = true;
            }

            Squares.AsParallel().ForAll(square =>
            {
                square.NeighboringBombs = SquareNeighbors(square).Count(s => s.IsBomb);
            });
            _boardInit = true;
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