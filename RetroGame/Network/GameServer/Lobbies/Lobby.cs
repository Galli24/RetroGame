using GameServer.Game;
using GameServer.Server;
using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Lobbies
{
    class Lobby
    {
        public string Name { get; }
        public bool HasPassword { get; }
        public string Password { get; }
        public ushort Slots { get; }
        public Dictionary<string, Player> Players { get; }

        private readonly GameManager _gameManager;

        public Lobby(string name, bool hasPassword, string password, ushort slots, SocketState host)
        {
            Name = name;
            HasPassword = hasPassword;
            Password = password;
            Slots = slots;

            Players = new Dictionary<string, Player>
            {
                { host.Username, new Player(host, true) }
            };

            _gameManager = new GameManager();
        }

        /*public bool AuthenticatedClient(Socket client, byte clientId)
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
        }*/

        public void StartGame(SocketState state)
        {
            var player = Players[state.Username];

            if (player.IsHost && !_gameManager.Started)
            {
                // TODO: Requires GameStartedMessage
                // foreach (var p in _players)
                //    TCPServer.SendServerMessage(p.State.Socket, )
                _gameManager.Start();
            }
        }

        public void EnqueueGameMessage(ClientMessage message)
        {
            if (_gameManager.Started)
                _gameManager.EnqueueMessage(message);
        }

        public void PlayerJoin(SocketState newPlayer)
        {
            foreach (var player in Players.Values)
            {
                new ServerLobbyPlayerJoinedMessage(player.State.Socket, newPlayer.Username).Send();
                new ServerLobbyPlayerReadyMessage(newPlayer.Socket, player.State.Username, player.IsReady).Send();
            }
            Players.Add(newPlayer.Username, new Player(newPlayer, false));
            new ServerLobbyJoinedMessage(newPlayer.Socket, true, string.Empty, Name, HasPassword, Slots, Players.Keys.ToList()).Send();
        }
        public void PlayerLeave(SocketState leftPlayer)
        {
            Players.Remove(leftPlayer.Username);
            foreach (var player in Players.Values)
                new ServerLobbyPlayerLeftMessage(player.State.Socket, leftPlayer.Username).Send();
        }

        public void PlayerReady(SocketState readyPlayer, bool isReady)
        {
            Players[readyPlayer.Username].IsReady = isReady;

            foreach (var player in Players.Values)
            {
                if (player.State.Username != readyPlayer.Username)
                    new ServerLobbyPlayerReadyMessage(player.State.Socket, readyPlayer.Username, isReady).Send();
            }
        }
    }
}
