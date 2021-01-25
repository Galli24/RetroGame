using RenderEngine;
using RetroGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RetroGame.Model
{
    class Scene1 : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        public Scene2 Tamer { get; set; }


        public Scene1() : base() { }


        public override void BuildScene()
        {
            var sc = RenderService.Instance;
            var button = new Button(sc.Window.Size / 2, "Scene1", IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            button.OnMousePress += (_, __, ___) => SceneManager.Instance.LoadScene(Tamer);
            _menu = new List<IMenu> { button };
        }

    }
}
