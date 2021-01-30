using LibNetworking.Messages.Server;
using LibNetworking.Models;
using RetroGame.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private float _tickRate = 1; 
        public float TickRateDeltaTime => 1 / _tickRate;
        public Dictionary<string, Player> Players = new Dictionary<string, Player>();

        public event EventHandler OnPlayerUpdated;

        #endregion

        public void StartGame(float tickRate)
        {
            _tickRate = tickRate;
            Players.Clear();
            foreach (var p in LobbyManager.Instance.PlayerList)
                Players.Add(p.Key, p.Value);

             RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new GameScene()));
        }

        public void OnPlayerUpdate(Player player)
        {
            player.LastRenderedPosition = Players[player.Name].Position;
            Players[player.Name] = player;
            OnPlayerUpdated?.Invoke(player, EventArgs.Empty);
        }

        public void OnSyncSnapshot(ServerGameSyncSnapshotMessage message)
        {
            // Player sync
            foreach (var player in message.PlayerList)
            {
                player.LastRenderedPosition = player.Position;
                Players[player.Name] = player;
            }
        }
    }
}
