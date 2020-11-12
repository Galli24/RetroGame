using GameServer.Game;
using LibNetworking.Messages.Client;
using System.Net.Sockets;

namespace GameServer.Lobby
{
    class Lobby
    {
        private readonly ushort _lobbyId;
        private readonly Socket _host;
        private readonly Socket _player;

        private bool _hostIsReady;
        private bool _playerIsReady;

        private readonly GameManager _gameManager;

        public Lobby(ushort lobbyId, Socket host, Socket player)
        {
            _lobbyId = lobbyId;
            _host = host;
            _player = player;
            _gameManager = new GameManager(host, player);
        }

        public bool AuthenticatedClient(Socket client, byte clientId)
        {
            if ((client == _host && clientId == 0) || (client == _player && clientId == 1))
                return true;
            return false;
        }

        public void ClientReady(byte clientId, bool ready)
        {
            if (clientId == 0)
            {
                if (_hostIsReady != ready)
                {
                    _hostIsReady = ready;
                    // TODO
                    // TCPServer.LobbyReady(_player, clientId, ready);
                }
            }
            else if (clientId == 1)
            {
                if (_playerIsReady != ready)
                {
                    _playerIsReady = ready;
                    // TODO
                    // TCPServer.LobbyReady(_host, clientId, ready);
                }
            }
        }

        public void StartGame(byte clientId)
        {
            if (clientId == 0 && _hostIsReady && _playerIsReady && !_gameManager.Started)
            {
                // TODO
                // TCPServer.GameStarted(_host);
                // TCPServer.GameStarted(_player);
                _gameManager.Start();
            }
        }

        public void EnqueueGameMessage(ClientMessage message)
        {
            if (_gameManager.Started)
                _gameManager.EnqueueMessage(message);
        }
    }
}
