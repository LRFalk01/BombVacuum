using System;
using System.Collections.Concurrent;
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
    }
}