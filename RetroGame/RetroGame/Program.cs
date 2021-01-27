using RetroGame.Scenes;
using RetroGame.Services;

namespace RetroGame
{
    class Program
    {
        static void Main()
        {
            RenderService.Instance.Init(); 
            RenderService.Instance.SetFPSVisibility(true);
            var loginScene = new LoginScene();
            SceneManager.Instance.LoadScene(loginScene);

            while (RenderService.Instance.RenderFrame())
            {
                // TODO: Le jeu
            }
        }
    }
}
