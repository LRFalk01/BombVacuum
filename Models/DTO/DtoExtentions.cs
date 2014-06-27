using System.Collections.Generic;
using System.Linq;
using BombVacuum.Models.Game;

namespace BombVacuum.Models.DTO
{
    public static class DtoExtentions
    {
        public static PlayerDTO ToDto(this Player player)
        {
            if (player == null) return null;
            return new PlayerDTO { Name = player.Name };
        }

        public static List<PlayerDTO> ToDto(this IEnumerable<Player> players)
        {
            if (players == null) return null;
            return players.Select(player => player.ToDto()).ToList();
        }

        public static GameDTO ToDto(this Game.Game game)
        {
            if (game == null) return null;
            return new GameDTO { Id = game.Id, Players = game.Players.ToDto() };
        }

        public static List<GameDTO> ToDto(this IEnumerable<Game.Game> games)
        {
            if (games == null) return null;
            return games.Select(game => game.ToDto()).ToList();
        }

        public static SquareDTO ToDto(this Square square)
        {
            if (square == null) return null;
            return new SquareDTO
            {
                Bomb = square.IsBomb, Column = square.Column, Row = square.Row, State = square.State, NeighboringBombs = square.State == SquareState.Unknown ? new int?() : square.NeighboringBombs
            
            };
        }

        public static List<SquareDTO> ToDto(this IEnumerable<Square> squares)
        {
            if (squares == null) return null;
            return squares.Select(Square => Square.ToDto()).ToList();
        }

        public static BoardDTO ToDto(this Board board)
        {
            if (board == null) return null;
            return new BoardDTO(board.Bombs, board.Squares);
        }

        public static List<BoardDTO> ToDto(this IEnumerable<Board> boards)
        {
            if (boards == null) return null;
            return boards.Select(board => board.ToDto()).ToList();
        }

    }
}