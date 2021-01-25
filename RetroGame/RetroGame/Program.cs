using RetroGame.Model;
using RetroGame.Services;
using System;

namespace RetroGame
{
    class Program
    {
        static void Main()
        {
            RenderService.Instance.Init(); // pd
            RenderService.Instance.SetFPSVisibility(true);
            var s1 = new Scene1();
            var s2 = new Scene2() { Tamer = s1 };
            s1.Tamer = s2;
            SceneManager.Instance.LoadScene(s1);

            while (RenderService.Instance.RenderFrame())
            {
                // TODO: Le jeu
            }
        }
    }
}
