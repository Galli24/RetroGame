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
    class Scene2 : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        public Scene1 Tamer { get; set; }
        
        public Scene2() : base() { }


        public override void BuildScene()
        {
            var sc = RenderService.Instance;
            var button = new Button(sc.Window.Size / 2, "Scene2", IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            button.OnMousePress += (_, __, ___) => SceneManager.Instance.LoadScene(Tamer);

            _menu = new List<IMenu> { button };
        }

    }
}
