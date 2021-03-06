﻿using LibNetworking.Messages.Client;
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
                case ClientMessageType.GAME_PLAYER_ACTION_STATES:
                    //Console.WriteLine("Request GAME_PLAYER_POSITION");
                    OnGamePlayerKeyState(client, message as ClientGamePlayerActionStatesMessage);
                    break;
                default:
                    return;
            }
        }

        private static void OnGamePlayerKeyState(SocketState client, ClientGamePlayerActionStatesMessage message)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromUsername(client.Username);

            if (lobby != null)
                lobby.EnqueueGameMessage(client, message, message.CurrentClientTick);
        }
    }
}
