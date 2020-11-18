using GameServer.Server;
using LibNetworking.Messages.Client;

namespace GameServer.Handlers
{
    class ClientMessageHandler
    {

        #region Constructor

        public ClientMessageHandler()
        {
            GlobalManager.Instance.Server.OnClientMessage += OnMessage;
        }

        #endregion

        #region Logic

        private void OnMessage(SocketState client, ClientMessage message)
        {
            switch (message.MessageTarget)
            {
                case MessageTarget.CONNECT:
                    ConnectMessageHandler.OnConnectMessage(client, message);
                    break;
                case MessageTarget.LOBBBY:
                    if (client.IsAuthenticated)
                    {
                        // TODO: Lobby message messages & handler
                    }
                    break;
                case MessageTarget.GAME:
                    if (client.IsAuthenticated)
                    {
                        // TODO: Game messages & handler
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
