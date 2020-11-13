using System.Collections.Generic;
using System.Net.Sockets;

namespace GameServer.Lobby
{
    // TODO: Rework
    class LobbyManager
    {
        private readonly Dictionary<ushort, Socket> _searchingClients;
        private readonly Dictionary<ushort, Lobby> _lobbies;

        public LobbyManager()
        {
            _lobbies = new Dictionary<ushort, Lobby>();
            _searchingClients = new Dictionary<ushort, Socket>();
        }

        public Lobby GetLobby(ushort lobbyId)
        {
            if (_lobbies.ContainsKey(lobbyId))
                return _lobbies[lobbyId];
            return null;
        }

        // TODO: Rework
        public void HandleLobbyJoinRequest(Socket player, ushort lobbySearchCode)
        {
            if (!_searchingClients.ContainsKey(lobbySearchCode) && !_lobbies.ContainsKey(lobbySearchCode))
            {
                _searchingClients.Add(lobbySearchCode, player);
            }
            else if (_searchingClients.ContainsKey(lobbySearchCode) && !_lobbies.ContainsKey(lobbySearchCode))
            {
                var host = _searchingClients[lobbySearchCode];
                _searchingClients.Remove(lobbySearchCode);
                _lobbies.Add(lobbySearchCode, new Lobby(lobbySearchCode, host, player));

                // TODO
                // TCPServer.LobbyFound(host, true, lobbySearchCode, 0);
                // TCPServer.LobbyFound(player, true, lobbySearchCode, 1);
            }
            else
            {
                // TODO
                // TCPServer.LobbyFound(player, false, lobbySearchCode, 0);
            }
        }
    }
}