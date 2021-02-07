using GameServer.Physics;
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
        private readonly float _fixedDeltaTime;
        private readonly Timer _gameLoopTimer;
        private long _currentTick;

        public bool Started { get; private set; }

        private readonly Dictionary<string, ServerPlayer> _players = new Dictionary<string, ServerPlayer>();
        private readonly List<ServerBullet> _bullets = new List<ServerBullet>();

        private PhysicsManager _physicsManager = new PhysicsManager();

        #endregion

        public GameManager()
        {
            _messages = new ConcurrentDictionary<long, Queue<ClientGameMessage>>();

            _gameLoopTimer = new Timer();
            _gameLoopTimer.Elapsed += (_, __) => GameLoop();
            _gameLoopTimer.Interval = _physicsInterval.TotalMilliseconds;
            _fixedDeltaTime = (float)_physicsInterval.TotalSeconds;
        }

        public void Start(Dictionary<string, Player> players)
        {

            foreach (var player in players)
            {
                _players.Add(player.Key, new ServerPlayer(player.Value.State, this));
                _physicsManager.RegisterItem(_players[player.Key].Id, new Vector2(100));
            }

            Started = true;

            foreach (var player in _players.Values)
                new ServerLobbyStartedMessage(player.State.Socket, true, PHYSICS_TICK_RATE).Send();

            _currentTick = 0;
            _gameLoopTimer.Start();
        }

        public void Stop()
        {
            _gameLoopTimer.Stop();
        }

        private void GameLoop()
        {
            // Handle packets received since last tick(s)
            HandleMessages(_currentTick);

            // Entity updates

            // Players
            foreach (var player in _players.Values)
                player.Update(_fixedDeltaTime);

            // Bullets
            foreach (var bullet in _bullets.ToArray())
                if (!bullet.ShouldDestroy)
                    bullet.Update(_fixedDeltaTime);
                else
                    RemoveBullet(bullet);

            // TODO: Enemies

            DoPhysics();

            if (_currentTick % SNAPSHOT_TICK_RATE_DIVIDE == 0)
                SendSnapshot();

            _currentTick++;
        }

        private void DoPhysics()
        {
            var playerGroup = new Dictionary<Guid, Vector2>();
            foreach (var p in _players)
                playerGroup.Add(p.Value.Id, p.Value.Position);


            var enemiesGroup = new Dictionary<Guid, Vector2>();
            foreach (var b in _bullets)
                enemiesGroup.Add(b.Id, b.Position);

            //foreach (var e in _enemies)
            //    enemiesGroup.Add(e.Id, e.Position);

            var collisions = _physicsManager.ComputeCollisions(playerGroup, enemiesGroup);

            foreach (var col in collisions)
            {
                var bullet = _bullets.FirstOrDefault(elt => elt.Id == col.Value);
                var player = _players.Values.FirstOrDefault(elt => elt.Id == col.Key);
                //var enemy = _enemies.FirstOrDefault(elt => elt.Id == col.Value);

                if (bullet != null)
                {
                    bullet.ShouldDestroy = true;
                    Console.WriteLine($"Hitting {player.Name}");
                    // damage col.Value (enemy)
                }
                else if (player != null)
                {
                    // take damage (got hit by smt)
                }
            }

        }

        public void AddBullet(Vector2 position)
        {
            var bullet = new ServerBullet(Guid.NewGuid(), position);
            _bullets.Add(bullet);
            _physicsManager.RegisterItem(bullet.Id, new Vector2(32, 48));
        }

        public void RemoveBullet(ServerBullet bullet)
        {
            _bullets.Remove(bullet);
            _physicsManager.UnregisterItem(bullet.Id);
        }

        public void PlayerLeft(string player)
        {
            if (_players.Keys.Contains(player))
                _players.Remove(player);
        }

        #region Netcode logic

        public void EnqueueMessage(SocketState client, ClientMessage message, long tick)
        {
            if (_currentTick >= tick)
            {
                new ServerGameSyncClockMessage(client.Socket, _currentTick + (_currentTick - tick + 20)).Send();
                return;
            }
            else if (tick >= _currentTick + 50)
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
            var bulletList = _bullets.ToArray();

            foreach (var player in _players.Values)
                new ServerGameSyncSnapshotMessage(player.State.Socket, _currentTick, playerList, bulletList).Send();
        }

        #endregion
    }
}
