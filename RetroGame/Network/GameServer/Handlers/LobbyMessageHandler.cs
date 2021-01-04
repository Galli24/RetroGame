using GameServer.Lobbies;
using GameServer.Server;
using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System;
using System.Collections.Generic;

namespace GameServer.Handlers
{
    class LobbyMessageHandler
    {
        public static void OnLobbyMessage(SocketState client, ClientMessage message)
        {
            switch (message.ClientMessageType)
            {
                case ClientMessageType.LOBBY_CREATE:
                    Console.WriteLine("Request LOBBY_CREATE");
                    OnLobbyCreate(client, (ClientLobbyCreateMessage)message);
                    break;
                case ClientMessageType.LOBBY_JOIN:
                    Console.WriteLine("Request LOBBY_JOIN");
                    OnLobbyJoin(client, (ClientLobbyJoinMessage)message);
                    break;
                case ClientMessageType.LOBBY_LEAVE:
                    Console.WriteLine("Request LOBBY_LEAVE");
                    OnLobbyLeave(client);
                    break;
                case ClientMessageType.LOBBY_READY:
                    Console.WriteLine("Request LOBBY_READY");
                    OnLobbyReady(client, (ClientLobbyReadyMessage)message);
                    break;
                default:
                    return;
            }
        }

        private static void OnLobbyCreate(SocketState client, ClientLobbyCreateMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.CreateLobby(message.Name, message.HasPassword, message.Password, message.Slots, client);

            if (lobby == null)
                new ServerLobbyCreatedMessage(client.Socket, false, "Already exists", string.Empty, false, 0).Send();
            else
                new ServerLobbyCreatedMessage(client.Socket, true, string.Empty, lobby.Name, lobby.HasPassword, lobby.Slots).Send();
        }

        private static void OnLobbyJoin(SocketState client, ClientLobbyJoinMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromName(message.Name);
            if (lobby == null)
                new ServerLobbyJoinedMessage(client.Socket, false, "Does not exist", string.Empty, false, 0, new List<string>()).Send();
            else
            {
                if (lobby.HasPassword && message.Password == lobby.Password)
                    lobby.PlayerJoin(client);
                else
                    new ServerLobbyJoinedMessage(client.Socket, false, "Wrong password", string.Empty, false, 0, new List<string>()).Send();
            }
        }

        private static void OnLobbyLeave(SocketState client)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);
            if (lobby != null)
                lobby.PlayerLeave(client);
        }

        private static void OnLobbyReady(SocketState client, ClientLobbyReadyMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);
            if (lobby != null)
                lobby.PlayerReady(client, message.Ready);
        }
    }
}
