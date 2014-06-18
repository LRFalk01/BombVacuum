using System.Collections.Generic;

namespace BombVacuum.Models.DTO
{
    public class GameDTO
    {
        public List<PlayerDTO> Players { get; set; }
        public string Id { get; set; }
    }
}