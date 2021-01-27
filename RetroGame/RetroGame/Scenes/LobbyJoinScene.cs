using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System.Collections.Generic;
using System.Numerics;

namespace RetroGame.Scenes
{
    class LobbyJoinScene : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        private LobbyMenuScene _lobbyMenuScene;

        private Button _joinButton;
        private Button _backButton;
        private TextBlock _joinBlock;
        private TextBlock _errorBlock;

        public LobbyJoinScene(LobbyMenuScene lobbyMenuScene) : base()
        {
            _lobbyMenuScene = lobbyMenuScene;
        }

        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y),
                "Join a lobby",
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 100], Vector2.One * 10);

            var nameBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .16f * sc.Window.Size.Y),
                "Lobby name",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var nameBox = new TextBox(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .1f * sc.Window.Size.Y),
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 28], Vector2.One * 10, 250);

            var passwordBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2)),
                "Password (can be empty)",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var passwordBox = new TextBox(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .06f * sc.Window.Size.Y),
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 28], Vector2.One * 10, 250);

            _joinButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Join lobby",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            _joinBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Joining...",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                BorderColor = new Vector4(1, 1, 1, 1)
            };

            _backButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .24f * sc.Window.Size.Y),
                "Back",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);

            _errorBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .32f * sc.Window.Size.Y),
                "",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                TextColor = new Vector4(1, 0, 0, 1)
            };

            _joinButton.OnMousePress += (_, __, ___) =>
            {
                _joinButton.BorderColor = new Vector4(.5f, .5f, .5f, 1);
                _menu.Remove(_joinButton);
                _menu.Remove(_backButton);
                _menu.Add(_joinBlock);
                _errorBlock.Text = "";
                Reload();
                NetworkManager.Instance.JoinLobby(nameBox.Text, passwordBox.Text);
            };

            _backButton.OnMousePress += (_, __, ___) =>
            {
                SceneManager.Instance.LoadScene(_lobbyMenuScene);
            };

            _menu = new List<IMenu> { title, nameBlock, nameBox, passwordBlock, passwordBox, _joinButton, _backButton, _errorBlock };
        }

        public void OnJoinFailed(string error)
        {
            _menu.Remove(_joinBlock);
            _menu.Add(_joinButton);
            _menu.Add(_backButton);
            _errorBlock.Text = error;
            Reload();
        }

    }
}