using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BombVacuum.Models.DTO;
using BombVacuum.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BombVacuum.Models.Game
{
    public sealed class GameState
    {
        private static readonly Lazy<GameState> _instance =
            new Lazy<GameState>(() => new GameState(GlobalHost.ConnectionManager.GetHubContext<GameHub>()));

        public static GameState Instance { get { return _instance.Value; } }

        private GameState(IHubContext context)
        {
            Clients = context.Clients;
            Groups = context.Groups;
        }

        private readonly ConcurrentDictionary<string, Player> _players =
            new ConcurrentDictionary<string, Player>(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentDictionary<string, Game> _games =
            new ConcurrentDictionary<string, Game>(StringComparer.OrdinalIgnoreCase);

        public IHubConnectionContext Clients { get; set; }
        public IGroupManager Groups { get; set; }


        public Player PlayerJoin(string userName, string userId)
        {
            if (_players.ContainsKey(userId)) return _players[userId];
            var player = new Player(userName, userId);
            _players[userId] = player;
            return player;
        }

        public bool PlayerLeave(string userId)
        {
            if (!_players.ContainsKey(userId)) return false;
            var player = _players[userId];
            if (!String.IsNullOrWhiteSpace(player.Group)) LeaveGame(userId, player.Group);
            _players.TryRemove(userId, out player);
            return true;
        }

        public Player GetPlayer(string name)
        {
            return _players.Values.FirstOrDefault(p => p.Name == name);
        }

        public Game GetGame(string id)
        {
            if (!_games.ContainsKey(id)) return null;
            return _games[id];
        }

        public Game JoinGame(string userId, string gameId)
        {
            if (!_games.ContainsKey(gameId) || !_players.ContainsKey(userId)) return null;
            var game = _games[gameId];
            var player = _players[userId];
            if (!String.IsNullOrWhiteSpace(player.Group)) LeaveGame(userId, player.Group);
            Groups.Add(player.ConnectionId, game.Id);
            game.Players.Add(player);
            player.Group = game.Id;

            Clients.Group(game.Id).gamePlayers(game.Players.ToDto());
            Clients.Client(player.ConnectionId).initGameBoard(game.Board.ToDto());
            return game;
        }

        public Game LeaveGame(string userId, string groupId)
        {
            if (!_players.ContainsKey(userId)) return null;
            var player = _players[userId];
            if (player.Group != groupId) return null;

            var game = _games[groupId];
            game.Players.Remove(player);
            Groups.Remove(player.ConnectionId, groupId);
            player.Group = null;
            if(!game.Players.Any()) DestroyGame(game.Id);

            Clients.Group(game.Id).gamePlayers(game.Players.ToDto());
            return game;
        }

        public Game CreateGame(string userId)
        {
            if (!_players.ContainsKey(userId)) return null;
            var player = _players[userId];
            if (!String.IsNullOrWhiteSpace(player.Group)) LeaveGame(userId, player.Group);
            var game = new Game(16, 16, 40);
            _games[game.Id] = game;
            Clients.All.updateGameList(_games.Values.ToDto());
            JoinGame(userId, game.Id);
            return game;
        }

        public ICollection<Game> CurrentGames()
        {
            return _games.Values;
        } 

        public void DestroyGame(string gameId)
        {
            if (!_games.ContainsKey(gameId)) return;
            var game = _games[gameId];
            foreach (var player in game.Players)
            {
                LeaveGame(player.UserId, game.Id);
            }
            _games.TryRemove(gameId, out game);
            Clients.All.updateGameList(_games.Values.ToDto());
        }

        public List<Square> RevealSquare(byte row, byte col, string userId)
        {
            if (!_players.ContainsKey(userId)) return null;
            var player = _players[userId];
            if (String.IsNullOrWhiteSpace(player.Group)) return null;
            if (!_games.ContainsKey(player.Group)) return null;
            var game = _games[player.Group];
            return game.Board.Reveal(row, col);
        }

        public Square FlagSquare(byte row, byte col, string userId)
        {
            if (!_players.ContainsKey(userId)) return null;
            var player = _players[userId];
            if (String.IsNullOrWhiteSpace(player.Group)) return null;
            if (!_games.ContainsKey(player.Group)) return null;
            var game = _games[player.Group];
            return game.Board.Flag(row, col);
        } 
    }
}