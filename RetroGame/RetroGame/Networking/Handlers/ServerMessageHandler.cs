using LibNetworking.Messages;
using LibNetworking.Messages.Server;
using RetroGame.Services;
using System;

namespace RetroGame.Networking.Handlers
{
    class ServerMessageHandler
    {

        #region Constructor

        public ServerMessageHandler(TCPClient tcpClient)
        {
            tcpClient.OnServerMessage += OnMessage;
        }

        #endregion

        #region Logic

        private void OnMessage(ServerMessage message)
        {
            NetworkManager.Instance.Ping = (float)Math.Round((DateTime.UtcNow - message.Time).TotalMilliseconds, 0);

            switch (message.MessageTarget)
            {
                case MessageTarget.CONNECT:
                    ConnectMessageHandler.OnConnectMessage(message);
                    break;
                case MessageTarget.LOBBY:
                    LobbyMessageHandler.OnLobbyMessage(message);
                    break;
                case MessageTarget.GAME:
                    GameMessageHandler.OnGameMessage(message);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
