using RetroGame.Model;
using RetroGame.Scenes;
using System.Collections.Generic;
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

        public Dictionary<string, Player> Players = new Dictionary<string, Player>();

        #endregion

        public void StartGame()
        {
            Players.Clear();
            foreach (var p in LobbyManager.Instance.PlayerList)
                Players.Add(p.Key, p.Value);

             RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new GameScene()));
        }

        public void OnPlayerPositionUpdate(string player, Vector2 position)
        {
            Players[player].Position = position;
        }
    }
}
