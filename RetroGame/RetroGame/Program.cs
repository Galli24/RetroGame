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
                // TODO: Le jeu
            }
        }
    }
}
