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
                    OnLobbyCreate(client, message as ClientLobbyCreateMessage);
                    break;
                case ClientMessageType.LOBBY_JOIN:
                    Console.WriteLine("Request LOBBY_JOIN");
                    OnLobbyJoin(client, message as ClientLobbyJoinMessage);
                    break;
                case ClientMessageType.LOBBY_READY:
                    Console.WriteLine("Request LOBBY_READY");
                    OnLobbyReady(client, message as ClientLobbyReadyMessage);
                    break;
                case ClientMessageType.LOBBY_START:
                    Console.WriteLine("Request LOBBY_START");
                    OnLobbyStart(client);
                    break;
                case ClientMessageType.LOBBY_LEAVE:
                    Console.WriteLine("Request LOBBY_LEAVE");
                    OnLobbyLeave(client);
                    break;
                default:
                    return;
            }
        }

        private static void OnLobbyCreate(SocketState client, ClientLobbyCreateMessage message)
        {
            if (string.IsNullOrEmpty(message.Name))
            {
                new ServerLobbyCreatedMessage(client.Socket, false, "Lobby name should not be empty", string.Empty, false, 0).Send();
                return;
            }

            var lobby = GlobalManager.Instance.LobbyManager.CreateLobby(message.Name, message.HasPassword, message.Password, message.Slots, client);

            if (lobby == null)
                new ServerLobbyCreatedMessage(client.Socket, false, "Lobby already exists", string.Empty, false, 0).Send();
            else
                new ServerLobbyCreatedMessage(client.Socket, true, string.Empty, lobby.Name, lobby.HasPassword, lobby.Slots).Send();
        }

        private static void OnLobbyJoin(SocketState client, ClientLobbyJoinMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromName(message.Name);
            if (lobby == null)
                new ServerLobbyJoinedMessage(client.Socket, false, "Lobby does not exist", string.Empty, false, 0, new List<string>()).Send();
            else
            {
                if ((lobby.HasPassword && message.Password == lobby.Password) || !lobby.HasPassword)
                    lobby.PlayerJoin(client);
                else
                    new ServerLobbyJoinedMessage(client.Socket, false, "Wrong password", string.Empty, false, 0, new List<string>()).Send();
            }
        }

        private static void OnLobbyReady(SocketState client, ClientLobbyReadyMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);
            if (lobby != null)
                lobby.PlayerReady(client, message.Ready);
        }
        private static void OnLobbyStart(SocketState client)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);
            if (lobby != null)
                lobby.StartGame(client);
        }

        private static void OnLobbyLeave(SocketState client)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);
            if (lobby != null)
                lobby.PlayerLeave(client);
        }
    }
}
