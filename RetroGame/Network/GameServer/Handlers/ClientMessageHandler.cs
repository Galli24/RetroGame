using GameServer.Server;
using GameServer.Utils;
using LibNetworking.Messages;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;

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
                case MessageTarget.LOBBY:
                    if (client.IsAuthenticated)
                        LobbyMessageHandler.OnLobbyMessage(client, message);
                    else
                        new ServerNotAuthenticatedMessage(client.Socket).Send();
                    break;
                case MessageTarget.GAME:
                    if (client.IsAuthenticated)
                    {
                        // TODO: Game messages & handler
                    } else
                        new ServerNotAuthenticatedMessage(client.Socket).Send();
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
