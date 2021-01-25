using RetroGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroGame.Services
{
    class SceneManager
    {

        #region Singleton

        private static SceneManager _instance;
        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SceneManager();

                return _instance;
            }
        }

        #endregion

        #region Members

        private Scene _currentScene;

        #endregion

        public SceneManager()
        {
            //RenderService.Instance.Window.OnResize += (_, __) => _currentScene?.OnResize();
        }

        #region Interface

        public void LoadScene(Scene scene)
        {
            _currentScene?.Exit();
            _currentScene = scene;
            _currentScene?.Enter();
        }

        #endregion

    }
}
