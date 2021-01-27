using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System.Collections.Generic;
using System.Numerics;

namespace RetroGame.Scenes
{
    class LobbyHostScene : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        private LobbyMenuScene _lobbyMenuScene;

        public LobbyHostScene(LobbyMenuScene lobbyMenuScene) : base()
        {
            _lobbyMenuScene = lobbyMenuScene;
        }

        private Button _createButton;
        private Button _backButton;
        private TextBlock _createBlock;
        private TextBlock _errorBlock;

        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y),
                "Host a lobby",
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

            _createButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Create lobby",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            _createBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Creating...",
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

            _createButton.OnMousePress += (_, __, ___) =>
            {
                _createButton.BorderColor = new Vector4(.5f, .5f, .5f, 1);
                _menu.Remove(_createButton);
                _menu.Remove(_backButton);
                _menu.Add(_createBlock);
                _errorBlock.Text = "";
                Reload();
                NetworkManager.Instance.CreateLobby(nameBox.Text, passwordBox.Text);
            };

           _backButton.OnMousePress += (_, __, ___) =>
            {
                SceneManager.Instance.LoadScene(_lobbyMenuScene);
            };

            _menu = new List<IMenu> { title, nameBlock, nameBox, passwordBlock, passwordBox, _createButton, _backButton, _errorBlock };
        }

        public void OnCreateFailed(string error)
        {
            _menu.Remove(_createBlock);
            _menu.Add(_createButton);
            _menu.Add(_backButton);
            _errorBlock.Text = error;
            Reload();
        }

    }
}