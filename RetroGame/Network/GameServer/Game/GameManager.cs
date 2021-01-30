using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using LibNetworking.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Timers;

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

        const float TICK_RATE = 64;
        private readonly TimeSpan _targetElsapsedTime = TimeSpan.FromSeconds(1 / TICK_RATE); // 64 ticks
        private readonly TimeSpan _syncSnapshotInterval = TimeSpan.FromSeconds(2);
        private Timer _gameLoopTimer;
        private Timer _syncSnapshotTimer;

        public bool Started { get; private set; }

        private readonly Dictionary<string, Player> _players = new Dictionary<string, Player>();

        #endregion

        public GameManager()
        {
            _messages = new ConcurrentQueue<ClientGameMessage>();
            _responses = new Queue<ServerMessage>();

            _gameLoopTimer = new Timer();
            _gameLoopTimer.Elapsed += (_, __) => GameLoop();
            _gameLoopTimer.Interval = _targetElsapsedTime.TotalSeconds;

            _syncSnapshotTimer = new Timer();
            _syncSnapshotTimer.Elapsed += (_, __) => SendSyncSnapshot();
            _syncSnapshotTimer.Interval = _syncSnapshotInterval.TotalSeconds;
        }

        public void Start(Dictionary<string, Player> players)
        {

            foreach (var player in players)
                _players.Add(player.Key, new Player(player.Value.State));

            Started = true;

            foreach (var player in _players.Values)
                new ServerLobbyStartedMessage(player.State.Socket, true, TICK_RATE).Send();

            _gameLoopTimer.Start();
            _syncSnapshotTimer.Start();
        }

        private void GameLoop()
        {
            // Handle packets received since last tick(s)
            HandleMessages();

            // Player inputs
            HandlePlayerActions((float)_targetElsapsedTime.TotalSeconds);

            // TODO: Do game simulation

            // Send player position packets for the current tick
            SendPlayerData();

            // Send packets for the current tick
            HandleResponses();
        }

        private void HandlePlayerActions(float deltaTime)
        {
            foreach (var player in _players.Values)
            {
                var p = Vector2.Zero;
                var speed = Player.SPEED;
                foreach (var action in player.ActionStates.Where(action => action.Value))
                {
                    switch (action.Key)
                    {
                        case Player.Actions.MOVE_LEFT:
                            p.X -= 1;
                            break;
                        case Player.Actions.MOVE_RIGHT:
                            p.X += 1;
                            break;
                        case Player.Actions.MOVE_DOWN:
                            p.Y -= 1;
                            break;
                        case Player.Actions.MOVE_UP:
                            p.Y += 1;
                            break;
                        case Player.Actions.BOOST:
                            speed = 1000;
                            break;

                    }
                }

                if (p != Vector2.Zero)
                    player.Position += p * deltaTime * speed;
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
                    case ClientMessageType.GAME_PLAYER_KEY_STATE:
                        HandleGamePlayerKeyState(message);
                        break;
                    default:
                        break;
                }
            }
        }

        #region Message handlers

        private void HandleGamePlayerKeyState(ClientGameMessage playerPositionMessage)
        {
            var client = playerPositionMessage.Client;
            var message = playerPositionMessage.Message as ClientGamePlayerKeyStateMessage;

            if (_players.ContainsKey(client.Username))
                _players[client.Username].ActionStates[message.KeyAction] = message.State;
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
                        if (receivingPlayer.Name != player.Name)
                            EnqueueResponse(new ServerGamePlayerUpdateMessage(receivingPlayer.State.Socket, player));
                    }

                    player.IsDirty = false;
                }
            }
        }

        private void SendSyncSnapshot()
        {
            var playerList = _players.Values.ToArray();

            foreach (var player in _players.Values)
                 EnqueueResponse(new ServerGameSyncSnapshotMessage(player.State.Socket, playerList));
        }

        #endregion
    }
}
