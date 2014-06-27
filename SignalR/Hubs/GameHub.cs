using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BombVacuum.Models.DTO;
using BombVacuum.Models.Game;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace BombVacuum.SignalR.Hubs
{

    [Authorize]
    public class GameHub : Hub
    {
        public void JoinServer()
        {
            var player = GameState.Instance.PlayerJoin(Context.User.Identity.Name, Context.User.Identity.GetUserId());
            player.ConnectionId = Context.ConnectionId;
            Clients.Caller.joinServer();
        }

        public void LeaveServer()
        {
            GameState.Instance.PlayerLeave(Context.User.Identity.GetUserId());
        }

        public void CurrentGames()
        {
            var games = GameState.Instance.CurrentGames();
            Clients.Caller.currentGames(games.ToDto());
        }

        public void JoinGame(string gameId)
        {
            if (String.IsNullOrEmpty(gameId)) return;
            var game = GameState.Instance.JoinGame(Context.User.Identity.GetUserId(), gameId);
            Clients.Caller.joinGame(game.ToDto());
        }

        public void CreateGame()
        {
            var game = GameState.Instance.CreateGame(Context.User.Identity.GetUserId());
            Clients.Caller.createGame(game.ToDto());
        }

        public void LeaveGame()
        {
            var player = GameState.Instance.GetPlayer(Context.User.Identity.Name);
            GameState.Instance.LeaveGame(player.UserId, player.Group);
        }

        public void InitGameBoard()
        {
            var player = GameState.Instance.GetPlayer(Context.User.Identity.Name);
            if (String.IsNullOrWhiteSpace(player.Group)) return;
            var game = GameState.Instance.GetGame(player.Group);
            if (game == null) return;
            Clients.Caller.initGameBoard(game.Board.ToDto());
        }

        public void Click(byte row, byte col)
        {
            var player = GameState.Instance.GetPlayer(Context.User.Identity.Name);
            if (String.IsNullOrWhiteSpace(player.Group)) return;
            var squares = GameState.Instance.RevealSquare(row, col, player.UserId);
            Clients.Group(player.Group).reveal(squares.ToDto());
        }

        public override Task OnDisconnected()
        {
            GameState.Instance.PlayerLeave(Context.User.Identity.GetUserId());
            return base.OnDisconnected();
        }

        public override Task OnConnected()
        {
            var player = GameState.Instance.PlayerJoin(Context.User.Identity.Name, Context.User.Identity.GetUserId());
            player.ConnectionId = Context.ConnectionId;
            Clients.Caller.joinServer();
            return base.OnConnected();
        }
    }
}