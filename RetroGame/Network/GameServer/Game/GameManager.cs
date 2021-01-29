using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using LibNetworking.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameServer.Game
{
    class GameManager
    {

        #region Netcode members

        private struct ClientGameMessage
        {
            public SocketState Client;
            public ClientMessage Message;

            public ClientGameMessage(SocketState client, ClientMessage message)
            {
                Client = client;
                Message = message;
            }
        }

        private readonly ConcurrentQueue<ClientGameMessage> _messages;
        private readonly Queue<ServerMessage> _responses;

        #endregion

        #region Members

        private readonly TimeSpan _targetElsapsedTime = TimeSpan.FromMilliseconds(15.625); // 64 ticks
        readonly GameClock _clock;
        public bool Started { get; private set; }
        private bool _isRunning;

        private readonly Dictionary<string, Player> _players = new Dictionary<string, Player>();

        #endregion

        public GameManager()
        {
            _messages = new ConcurrentQueue<ClientGameMessage>();
            _responses = new Queue<ServerMessage>();
            _clock = new GameClock();
        }

        public void Start(Dictionary<string, Player> players)
        {

            foreach (var player in players)
                _players.Add(player.Key, new Player(player.Value.State));

            Started = true;
            _clock.Restart();
            _isRunning = true;
            Task.Run(() => GameLoop());
        }

        private async void GameLoop()
        {
            while (_isRunning)
            {
                // Check how many ticks have passed
                _clock.Step();
                long ratio = _clock.Elapsed.Ticks / _targetElsapsedTime.Ticks;

                // Do logic if ticks have passed
                while (ratio > 0)
                {
                    ratio -= 1;

                    // Handle packets received since last tick(s)
                    HandleMessages();

                    // TODO: Do game simulation

                    // Send player position packets for the current tick
                    SendPlayerData();

                    // Send packets for the current tick
                    HandleResponses();

                }

                // Reduce CPU cycles
                await Task.Delay(1);
            }
        }

        #region Netcode logic

        public void EnqueueMessage(SocketState client, ClientMessage message) => _messages.Enqueue(new ClientGameMessage(client, message));

        private void HandleMessages()
        {
            while (!_messages.IsEmpty)
            {
                var dequeued = _messages.TryDequeue(out ClientGameMessage message);
                if (!dequeued) continue;

                switch (message.Message.ClientMessageType)
                {
                    case ClientMessageType.GAME_PLAYER_POSITION:
                        HandleGamePlayerPosition(message);
                        break;
                    default:
                        break;
                }
            }
        }

        #region Message handlers

        private void HandleGamePlayerPosition(ClientGameMessage playerPositionMessage)
        {
            var client = playerPositionMessage.Client;
            var message = playerPositionMessage.Message as ClientGamePlayerPositionMessage;

            if (_players.ContainsKey(client.Username))
                _players[client.Username].Position = message.Position;
        }

        #endregion

        private void EnqueueResponse(ServerMessage message) => _responses.Enqueue(message);

        private void HandleResponses()
        {
            while (_responses.Count > 0)
            {
                var dequeued = _responses.TryDequeue(out ServerMessage serverMessage);
                if (!dequeued) continue;
                serverMessage.Send();
            }
        }

        private void SendPlayerData()
        {
            // Player positions
            foreach (var player in _players.Values)
            {
                if (player.IsDirty)
                {
                    foreach (var receivingPlayer in _players.Values)
                    {
                        if (receivingPlayer.State.Username != player.State.Username)
                            EnqueueResponse(new ServerGamePlayerUpdateMessage(receivingPlayer.State.Socket, player));
                    }
                    player.IsDirty = false;
                }
            }
        }

        #endregion
    }
}
