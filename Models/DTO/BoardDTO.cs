using System.Collections.Generic;

namespace BombVacuum.Models.DTO
{
    public class BoardDTO
    {
        public byte Rows { get; set; }
        public byte Columns { get; set; }
        public int Bombs { get; set; }
        public List<SquareDTO> Squares { get; set; }
    }
}