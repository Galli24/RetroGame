using RetroGame.Scenes;
using RetroGame.Services;

namespace RetroGame
{
    class Program
    {
        static void Main()
        {
            RenderService.Instance.Init();
#if DEBUG
            RenderService.Instance.SetFPSVisibility(true);
#endif

            var loginScene = new LoginScene();
            var lobbyScene = new LobbyScene();
            SceneManager.Instance.LoadScene(loginScene);

            while (RenderService.Instance.RenderFrame())
            {
                if (SceneManager.Instance.CurrentScene is not GameScene scene)
                    continue;
                scene.Update(RenderService.Instance.FrameTime);
            }

            if (SceneManager.Instance.CurrentScene is GameScene gameScene)
                gameScene.Stop();
        }
    }
}
