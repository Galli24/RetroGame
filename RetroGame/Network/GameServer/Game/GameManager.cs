using GameServer.Server;
using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Game
{
    // TODO
    class GameManager
    {

        #region Netcode members

        private readonly ConcurrentQueue<ClientMessage> _messages;
        private readonly Queue<ServerMessage> _responses;

        #endregion

        private readonly TimeSpan _targetElsapsedTime = TimeSpan.FromTicks(166667); // 60 ticks (1 per 16.67ms)
        readonly GameClock _clock;
        public bool Started { get; private set; }
        private bool _isRunning;

        public GameManager()
        {
            _messages = new ConcurrentQueue<ClientMessage>();
            _responses = new Queue<ServerMessage>();
            _clock = new GameClock();
        }

        public void Start()
        {
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

        public void EnqueueMessage(ClientMessage message)
        {
            _messages.Enqueue(message);
        }

        private void HandleMessages()
        {
            while (!_messages.IsEmpty)
            {
                var dequeued = _messages.TryDequeue(out ClientMessage message);
                if (!dequeued) continue;

                switch (message.MessageType)
                {
                    default:
                        break;
                }
            }
        }

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
