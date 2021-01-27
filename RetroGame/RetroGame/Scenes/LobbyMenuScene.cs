using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System.Collections.Generic;
using System.Numerics;

namespace RetroGame.Scenes
{
    class LobbyMenuScene : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        private LobbyHostScene _lobbyHostScene;
        private LobbyJoinScene _lobbyJoinScene;

        public LobbyMenuScene() : base()
        {
            _lobbyHostScene = new LobbyHostScene(this);
            _lobbyJoinScene = new LobbyJoinScene(this);
        }


        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y),
                "Lobbies",
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 100], Vector2.One * 10);

            var hostButton = new Button(new Vector2((sc.Window.Size.X / 2) - 0.1f * sc.Window.Size.X, sc.Window.Size.Y / 2),
               "Host a lobby",
               IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var joinButton = new Button(new Vector2((sc.Window.Size.X / 2) + 0.1f * sc.Window.Size.X, sc.Window.Size.Y / 2),
               "Join a lobby",
               IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);

            hostButton.OnMousePress+= (_, __, ___) =>
            {
                SceneManager.Instance.LoadScene(_lobbyHostScene);
            };

            joinButton.OnMousePress += (_, __, ___) =>
            {
                SceneManager.Instance.LoadScene(_lobbyJoinScene);
            };

            _menu = new List<IMenu> { title, hostButton, joinButton };
        }

    }
}