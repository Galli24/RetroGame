using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System.Collections.Generic;
using System.Numerics;

namespace RetroGame.Scenes
{
    class LobbyScene : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        public LobbyScene() : base() { }

        private string _error = "";

        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y),
                "Success woooooo",
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 100], Vector2.One * 10);

            _menu = new List<IMenu> { title };
        }
    }
}