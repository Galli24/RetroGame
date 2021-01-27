using LibNetworking.Messages;
using LibNetworking.Messages.Server;
using RetroGame.Services;

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
                    // TODO: Game messages & handler
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
