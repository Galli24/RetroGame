using LibNetworking.Messages;
using LibNetworking.Messages.Server;

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
