﻿using GameServer.Game;
using GameServer.Server;
using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Lobbies
{
    class Lobby
    {
        public string Name { get; }
        public bool HasPassword { get; }
        public string Password { get; }
        public ushort Slots { get; }
        public Dictionary<string, Player> Players { get; }

        private readonly GameManager _gameManager;

        public Lobby(string name, bool hasPassword, string password, ushort slots, SocketState host)
        {
            Name = name;
            HasPassword = hasPassword;
            Password = password;
            Slots = slots;

            Players = new Dictionary<string, Player>
            {
                { host.Username, new Player(host, true) }
            };

            _gameManager = new GameManager();
        }

        public void StartGame(SocketState state)
        {
            var startingPlayer = Players[state.Username];

            if (startingPlayer.IsHost && !_gameManager.Started)
            {
                if (!AllPlayersReady())
                {
                    new ServerLobbyStartedMessage(state.Socket, false, "Not all players are ready").Send();
                    return;
                }

                foreach (var player in Players.Values)
                    new ServerLobbyStartedMessage(player.State.Socket, true).Send();
                _gameManager.Start();
            }
        }

        private bool AllPlayersReady()
        {
            foreach (var player in Players.Values)
                if (!player.IsReady)
                    return false;
            return true;
        }

        public void EnqueueGameMessage(ClientMessage message)
        {
            if (_gameManager.Started)
                _gameManager.EnqueueMessage(message);
        }

        public void PlayerJoin(SocketState newPlayer)
        {
            foreach (var player in Players.Values)
            {
                new ServerLobbyPlayerJoinedMessage(player.State.Socket, newPlayer.Username).Send();
                new ServerLobbyPlayerReadyMessage(newPlayer.Socket, player.State.Username, player.IsReady).Send();
            }
            Players.Add(newPlayer.Username, new Player(newPlayer, false));
            new ServerLobbyJoinedMessage(newPlayer.Socket, true, string.Empty, Name, HasPassword, Slots, Players.Keys.ToList()).Send();
        }
        public void PlayerLeave(SocketState leftPlayer)
        {
            Players.Remove(leftPlayer.Username);
            foreach (var player in Players.Values)
                new ServerLobbyPlayerLeftMessage(player.State.Socket, leftPlayer.Username).Send();

            if (Players.Count == 0)
                GlobalManager.Instance.LobbyManager.RemoveLobby(Name);
        }

        public void PlayerReady(SocketState readyPlayer, bool isReady)
        {
            Players[readyPlayer.Username].IsReady = isReady;

            foreach (var player in Players.Values)
            {
                if (player.State.Username != readyPlayer.Username)
                    new ServerLobbyPlayerReadyMessage(player.State.Socket, readyPlayer.Username, isReady).Send();
            }
        }
    }
}
