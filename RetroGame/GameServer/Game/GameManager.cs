using GameServer.Server;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Game
{
    // TODO
    class GameManager
    {

        #region Netcode members

        private readonly Socket _host;
        private readonly Socket _player;

        private readonly ConcurrentQueue<ClientMessage> _messages;
        private readonly Queue<GameResponse> _responses;

        private class GameResponse
        {
            public Socket Receiver { get; private set; }
            public ServerMessage Response { get; private set; }

            public GameResponse(Socket receiver, ServerMessage response)
            {
                Receiver = receiver;
                Response = response;
            }
        }

        #endregion

        private readonly TimeSpan _targetElsapsedTime = TimeSpan.FromTicks(166667); // 60 ticks (1 per 16.67ms)
        readonly GameClock _clock;
        public bool Started { get; private set; }
        private bool _isRunning;

        public GameManager(Socket host, Socket player)
        {
            _host = host;
            _player = player;

            _messages = new ConcurrentQueue<ClientMessage>();
            _responses = new Queue<GameResponse>();
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
                var dequeued = _responses.TryDequeue(out GameResponse gameResponse);
                if (!dequeued) continue;
                TCPServer.SendServerMessage(gameResponse.Receiver, gameResponse.Response);
            }
        }

        #endregion
    }
}