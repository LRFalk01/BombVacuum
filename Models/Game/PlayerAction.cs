using System;

namespace BombVacuum.Models.Game
{
    public class PlayerAction
    {
        public byte Row { get; set; }
        public byte Column { get; set; }
        public ActionType ActionType { get; set; }
        public SquareStatus ActionResult { get; set; }
        public DateTime ActionTime { get; set; }
    }

    public enum ActionType
    {
        Click,
        Flag
    }
}