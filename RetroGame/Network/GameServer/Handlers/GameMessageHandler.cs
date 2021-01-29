using LibNetworking.Messages.Client;
using LibNetworking.Models;
using System;

namespace GameServer.Handlers
{
    class GameMessageHandler
    {
        public static void OnGameMessage(SocketState client, ClientMessage message)
        {
            switch (message.ClientMessageType)
            {
                case ClientMessageType.GAME_PLAYER_POSITION:
                    //Console.WriteLine("Request GAME_PLAYER_POSITION");
                    OnGamePlayerPosition(client, message as ClientGamePlayerPositionMessage);
                    break;
                default:
                    return;
            }
        }

        private static void OnGamePlayerPosition(SocketState client, ClientGamePlayerPositionMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);

            if (lobby != null)
                lobby.EnqueueGameMessage(client, message);
        }
    }
}
