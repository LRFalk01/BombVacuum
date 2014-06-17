using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BombVacuum.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BombVacuum.Models
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


        public Player PlayerJoin(ApplicationUser user)
        {
            if (_players.ContainsKey(user.Id)) return _players[user.Id];
            var player = new Player(user.UserName, user.Id);
            _players[user.Id] = player;
            return player;
        }

        public Player GetPlayer(string name)
        {
            return _players.Values.FirstOrDefault(p => p.Name == name);
        }

        public Player JoinGame(string userId, string gameId)
        {
            if (!_games.ContainsKey(gameId) || !_players.ContainsKey(userId)) return null;
            var game = _games[gameId];
            var player = _players[userId];
            if (!String.IsNullOrWhiteSpace(player.Group)) LeaveGame(userId, player.Group);
            Groups.Add(player.ConnectionId, game.Id);
            game.Players.Add(player);
            player.Group = game.Id;
            return player;
        }

        public Player LeaveGame(string userId, string groupId)
        {
            if (!_players.ContainsKey(userId)) return null;
            var player = _players[userId];
            if (player.Group != groupId) return null;
            Groups.Remove(player.ConnectionId, groupId);
            player.Group = null;
            return player;
        }

        public Game CreateGame(string userId)
        {
            if (!_players.ContainsKey(userId)) return null;
            var player = _players[userId];
            if (!String.IsNullOrWhiteSpace(player.Group)) LeaveGame(userId, player.Group);
            var game = new Game(16, 16, 40);
            player.Group = game.Id;
            game.Players.Add(player);
            Groups.Add(player.ConnectionId, game.Id);
            _games[game.Id] = game;
            return game;
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
        }

        public List<Square> RevealSquare(byte row, byte col, string userId)
        {
            if (!_players.ContainsKey(userId)) return null;
            var player = _players[userId];
            if (String.IsNullOrWhiteSpace(player.Group)) return null;
            if (!_games.ContainsKey(player.Group)) return null;
            var game = _games[player.Group];
            return game.Click(row, col);
        } 
    }
}