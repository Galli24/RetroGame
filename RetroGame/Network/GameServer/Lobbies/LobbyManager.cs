using LibNetworking.Models;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Lobbies
{
    class LobbyManager
    {
        private readonly Dictionary<string, Lobby> _lobbies;

        public LobbyManager()
        {
            _lobbies = new Dictionary<string, Lobby>();
        }

        public Lobby CreateLobby(string name, bool hasPassword, string password, ushort slots, SocketState host)
        {
            if (!string.IsNullOrEmpty(name) && !_lobbies.ContainsKey(name))
            {
                _lobbies.Add(name, new Lobby(name, hasPassword, password, slots, host));
                return _lobbies[name];
            }
            return null;
        }

        public void RemoveLobby(string name)
        {
            if (!string.IsNullOrEmpty(name) && _lobbies.ContainsKey(name))
                _lobbies.Remove(name);
        }

        public Lobby GetLobbyFromName(string name)
        {
            if (!string.IsNullOrEmpty(name) && _lobbies.ContainsKey(name))
                return _lobbies[name];
            return null;
        }

        public Lobby GetLobbyFromUsername(string username)
        {
            return _lobbies.Values.FirstOrDefault(lobby => lobby.Players.ContainsKey(username));
        }

        public Lobby GetLobbyFromState(SocketState state)
        {
            return _lobbies.Values.FirstOrDefault(lobby => lobby.Players.Values.Any(player => player.State == state));
        }
    }
}