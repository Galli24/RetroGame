using GameServer.Lobby;
using LibNetworking;
using LibNetworking.Messages.Client;
using System.Net.Sockets;

namespace GameServer.Server
{
    class ClientMessageHandler
    {
        private readonly LobbyManager _lobbyManager;

        public ClientMessageHandler(LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;

            NetworkCallbacks.OnClientMessage += OnClientMessage;
        }

        // TODO
        private void OnClientMessage(Socket client, ClientMessage message)
        {
            switch (message.MessageTarget)
            {
                case MessageTarget.CONNECT:
                    // TODO: Connection handler
                    break;
                case MessageTarget.LOBBBY:
                    // TODO: Lobby message messages & handler
                    break;
                case MessageTarget.GAME:
                    // TODO: Game messages & handler
                    break;
                default:
                    break;
            }
        }

        /*private Lobby.Lobby GetLobbyAndAuthClient(Socket client, ushort lobbyId, byte clientId)
        {
            var lobby = _globalManager.LobbyManager.GetLobby(lobbyId);
            if (lobby?.AuthenticatedClient(client, clientId) == true)
                return lobby;
            return null;
        }

        private void OnLobbyJoin(Socket client, JoinLobbyMessage message) => _globalManager.LobbyManager.HandleLobbyJoinRequest(client, message.LobbySearchCode);

        private void OnLobbyReady(Socket client, LobbyReadyMessage message)
        {
            var lobby = GetLobbyAndAuthClient(client, message.LobbyId, message.ClientId);
            lobby?.ClientReady(message.ClientId, message.Ready);
        }

        private void OnGameStart(Socket client, GameStartMessage message)
        {
            var lobby = GetLobbyAndAuthClient(client, message.LobbyId, message.ClientId);
            lobby?.StartGame(message.ClientId);
        }

        private void OnGameMessage(Socket client, ushort lobbyId, byte clientId, Message message)
        {
            var lobby = GetLobbyAndAuthClient(client, lobbyId, clientId);
            lobby?.EnqueueGameMessage(message);
        }*/
    }
}