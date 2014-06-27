using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using BombVacuum.Models.Game;

namespace BombVacuum.Models.DTO
{
    public class BoardDTO
    {
        public BoardDTO(int bombs, ICollection<Square> squares)
        {
            Rows = (byte)(squares.Max(s => s.Row) + 1);
            Columns = (byte)(squares.Max(s => s.Column) + 1);
            Bombs = bombs;
            Squares = new SquareDTO[Rows,Columns];

            for (byte row = 0; row < Rows; row++)
            {
                for (byte col = 0; col < Columns; col++)
                {
                    Squares[row, col] = squares.First(s => s.Row == row && s.Column == col).ToDto();
                }
            }
        }
        public byte Rows { get; private set; }
        public byte Columns { get; private set; }
        public int Bombs { get; private set; }
        public SquareDTO[,] Squares { get; private set; }
    }
}