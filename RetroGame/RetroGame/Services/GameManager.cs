using LibNetworking.Messages.Server;
using LibNetworking.Models;
using RetroGame.Scenes;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace RetroGame.Services
{
    class GameManager
    {
        #region Singleton

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }

        #endregion

        #region Members

        public long CurrentServerTick;
        public long CurrentClientTick;

        public Vector2 PositionDifference;

        private float _tickRate = 1; 
        public float TickRateDeltaTime => 1 / _tickRate;
        public Dictionary<string, Player> Players = new Dictionary<string, Player>();

        public readonly ConcurrentDictionary<long, Player> PlayerBufferedHistory = new ConcurrentDictionary<long, Player>();

        #endregion

        public void StartGame(float tickRate)
        {
            _tickRate = tickRate;
            CurrentServerTick = 0;
            CurrentClientTick = 0;
            Players.Clear();
            PlayerBufferedHistory.Clear();
            foreach (var p in LobbyManager.Instance.PlayerList)
                Players.Add(p.Key, p.Value);

            Players.Add("server difference", new Player());

            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new GameScene()));
        }

        public void OnSyncClock(long requestedClock)
        {
            CurrentClientTick = requestedClock;
        }

        public void OnSyncSnapshot(ServerGameSyncSnapshotMessage message)
        {
            CurrentServerTick = message.CurrentServerTick;
            var gotClientHistory = PlayerBufferedHistory.TryRemove(message.CurrentServerTick, out Player clientHistory);

            // Player reconciliation
            if (gotClientHistory)
            {
                var clientPlayer = message.PlayerList.Where(player => player.Name == UserManager.Instance.Username).First();

                var positionDifference = clientHistory.Position - clientPlayer.Position;
                PositionDifference = positionDifference;
                Players["server difference"].Position = clientPlayer.Position;

                if (positionDifference.X > 1 || positionDifference.Y > 1
                    || positionDifference.X < -1 || positionDifference.Y < -1)
                    ReconcilePlayerPosition(message.CurrentServerTick, clientPlayer.Position);
            }

            // Other player positions
            foreach (var player in message.PlayerList)
            {
                if (player.Name != UserManager.Instance.Username)
                {
                    player.LastRenderedPosition = Players[player.Name].Position;
                    player.LerpElapsed = 0;
                    player.LerpDuration = 1 * (1 / 22f);
                    Players[player.Name] = player;
                }
            }

            // Cleanup
            var keysToRemove = PlayerBufferedHistory.Keys.Where(key => key < message.CurrentServerTick).ToList();
            keysToRemove.ForEach(key => PlayerBufferedHistory.TryRemove(key, out _));
        }

        private void ReconcilePlayerPosition(long tick, Vector2 serverPosition)
        {
            Trace.WriteLine("Reconcile start");
            Vector2 reconciledPosition = serverPosition;
            var inputsToReplay = PlayerBufferedHistory.Where(kv => kv.Key > tick).OrderBy(kv => kv.Key).ToList();
            foreach (var bufferedPlayer in inputsToReplay)
            {
                reconciledPosition += ReconcileHandleInput(bufferedPlayer.Value);
                PlayerBufferedHistory[bufferedPlayer.Key].Position = reconciledPosition;
            }

            Players[UserManager.Instance.Username].Position = reconciledPosition;
            Trace.WriteLine("Reconcile end");
        }

        private Vector2 ReconcileHandleInput(Player bufferedPlayer)
        {
            var p = Vector2.Zero;
            var speed = Player.SPEED;

            foreach (var action in bufferedPlayer.ActionStates.Where(action => action.Value))
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

            return p * TickRateDeltaTime * speed;
        }
    }
}
