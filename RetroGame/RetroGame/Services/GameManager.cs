using RetroGame.Model;
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

        #endregion

        public void StartGame()
        {
            // TODO: init stuff then start
            // RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new GameScene()));
        }

        public void OnPlayerPositionUpdate(string player, Vector2 position)
        {
            // TODO
        }
    }
}
