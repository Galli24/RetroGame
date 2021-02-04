using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using LibNetworking.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        private readonly ConcurrentDictionary<long, Queue<ClientGameMessage>> _messages;

        #endregion

        #region Members

        const float PHYSICS_TICK_RATE = 66;
        const float SNAPSHOT_TICK_RATE_DIVIDE = 3;
        private readonly TimeSpan _physicsInterval = TimeSpan.FromSeconds(1 / PHYSICS_TICK_RATE);
        private Timer _gameLoopTimer;
        private long _currentTick;

        public bool Started { get; private set; }

        private readonly Dictionary<string, Player> _players = new Dictionary<string, Player>();

        #endregion

        public GameManager()
        {
            _messages = new ConcurrentDictionary<long, Queue<ClientGameMessage>>();

            _gameLoopTimer = new Timer();
            _gameLoopTimer.Elapsed += (_, __) => GameLoop();
            _gameLoopTimer.Interval = _physicsInterval.TotalMilliseconds;
        }

        public void Start(Dictionary<string, Player> players)
        {

            foreach (var player in players)
                _players.Add(player.Key, new Player(player.Value.State));

            Started = true;

            foreach (var player in _players.Values)
                new ServerLobbyStartedMessage(player.State.Socket, true, PHYSICS_TICK_RATE).Send();

            _currentTick = 0;
            _gameLoopTimer.Start();
        }

        private void GameLoop()
        {
            // Handle packets received since last tick(s)
            HandleMessages(_currentTick);

            // Player inputs
            HandlePlayerActions((float)_physicsInterval.TotalSeconds);

            // TODO: Physics

            if (_currentTick % SNAPSHOT_TICK_RATE_DIVIDE == 0)
                SendSnapshot();

            _currentTick++;
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

        public void EnqueueMessage(SocketState client, ClientMessage message, long tick)
        {
            if (_currentTick >= tick)
            {
                new ServerGameSyncClockMessage(client.Socket, _currentTick + (_currentTick - tick + 5)).Send();
                return;
            } else if (tick >= _currentTick + 20)
            {
                new ServerGameSyncClockMessage(client.Socket, (_currentTick + tick) / 2).Send();
                return;
            }

            if (_messages.ContainsKey(tick))
                _messages[tick].Enqueue(new ClientGameMessage(client, message));
            else
            {
                _messages.TryAdd(tick, new Queue<ClientGameMessage>());
                _messages[tick].Enqueue(new ClientGameMessage(client, message));
            }
        }

        private void HandleMessages(long tick)
        {
            if (_messages.ContainsKey(tick))
            {
                if (!_messages.TryRemove(tick, out Queue<ClientGameMessage> tickQueue))
                    return;

                while (tickQueue.Count > 0)
                {
                    var dequeued = tickQueue.Dequeue();
                    switch (dequeued.Message.ClientMessageType)
                    {
                        case ClientMessageType.GAME_PLAYER_ACTION_STATES:
                            HandleGamePlayerKeyState(dequeued);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region Message handlers

        private void HandleGamePlayerKeyState(ClientGameMessage playerPositionMessage)
        {
            var client = playerPositionMessage.Client;
            var message = playerPositionMessage.Message as ClientGamePlayerActionStatesMessage;

            if (_players.ContainsKey(client.Username))
                _players[client.Username].ActionStates = message.ActionStates;
        }

        #endregion

        private void SendSnapshot()
        {
            var playerList = _players.Values.ToArray();

            foreach (var player in _players.Values)
                new ServerGameSyncSnapshotMessage(player.State.Socket, _currentTick, playerList).Send();
        }

        #endregion
    }
}
