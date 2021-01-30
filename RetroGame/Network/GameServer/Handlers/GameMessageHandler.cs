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
                case ClientMessageType.GAME_PLAYER_KEY_STATE:
                    //Console.WriteLine("Request GAME_PLAYER_POSITION");
                    OnGamePlayerKeyState(client, message as ClientGamePlayerKeyStateMessage);
                    break;
                default:
                    return;
            }
        }

        private static void OnGamePlayerKeyState(SocketState client, ClientGamePlayerKeyStateMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);

            if (lobby != null)
                lobby.EnqueueGameMessage(client, message);
        }
    }
}
