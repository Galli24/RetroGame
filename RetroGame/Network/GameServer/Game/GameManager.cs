using GameServer.Lobbies;
using GameServer.Server;
using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Game
{
    // TODO
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

        private Lobby _gameLobby;

        private readonly TimeSpan _targetElsapsedTime = TimeSpan.FromTicks(166667); // 60 ticks (1 per 16.67ms)
        readonly GameClock _clock;
        public bool Started { get; private set; }
        private bool _isRunning;

        #endregion

        public GameManager(Lobby gameLobby)
        {
            _messages = new ConcurrentQueue<ClientGameMessage>();
            _responses = new Queue<ServerMessage>();
            _clock = new GameClock();
            _gameLobby = gameLobby;
        }

        public void Start()
        {
            foreach (var player in _gameLobby.Players.Values)
                player.Position = Vector2.Zero;

            Started = true;
            _clock.Restart();
            _isRunning = true;
            Task.Run(() => GameLoop());
        }

        private void GameLoop()
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

                    // Send packets for the current tick
                    HandleResponses();
                }

                // Reduce CPU cycles
                Thread.Sleep(1);
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

            _gameLobby.Players[client.Username].Position = message.Position;
            foreach (var player in _gameLobby.Players.Values)
            {
                if (player.State.Username != client.Username)
                    EnqueueResponse(new ServerGamePlayerPositionMessage(player.State.Socket, client.Username, message.Position));
            }
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

        #endregion
    }
}
