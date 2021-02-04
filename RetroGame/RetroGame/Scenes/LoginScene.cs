using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RetroGame.Scenes
{
    class LoginScene : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;
        public override IEnumerable<IGraphNode> Sprites => Array.Empty<IGraphNode>();

        private readonly RegisterScene _registerScene;

        private Button _loginButton;
        private Button _registerButton;
        private TextBlock _loginBlock;
        private TextBlock _errorBlock;

        public LoginScene() : base()
        {
            _registerScene = new RegisterScene(this);
        }


        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y),
                "RetroGame",
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 100], Vector2.One * 10);

            var usernameBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .16f * sc.Window.Size.Y),
                "Username",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var usernameBox = new TextBox(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .1f * sc.Window.Size.Y),
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 28], Vector2.One * 10, 250);

            var passwordBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2)),
                "Password",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var passwordBox = new TextBox(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .06f * sc.Window.Size.Y),
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 28], Vector2.One * 10, 250);

            _loginButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Login",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            _loginBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Logging in...",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                BorderColor = new Vector4(1, 1, 1, 1)
            };

            _registerButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .24f * sc.Window.Size.Y),
                "Register",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);

            _errorBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .32f * sc.Window.Size.Y),
                "",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                TextColor = new Vector4(1, 0, 0, 1)
            };

            _loginButton.OnMousePress += async(_, __, ___) =>
            {
                _loginButton.BorderColor = new Vector4(.5f, .5f, .5f, 1);
                _menu.Remove(_loginButton);
                _menu.Remove(_registerButton);
                _menu.Add(_loginBlock);
                _errorBlock.Text = "";
                Reload();
                var result = await NetworkManager.Instance.Connect(usernameBox.Text, passwordBox.Text);

                if (!string.IsNullOrEmpty(result))
                {
                    if (result != "Connected")
                    {
                        _menu.Remove(_loginBlock);
                        _menu.Add(_loginButton);
                        _menu.Add(_registerButton);
                        _errorBlock.Text = result;
                        Reload();
                    } else
                    {
                        // SceneManager.Instance.LoadScene();
                    }
                }
            };

            _registerButton.OnMousePress += (_, __, ___) =>
            {
                SceneManager.Instance.LoadScene(_registerScene);
            };

            _menu = new List<IMenu> { title, usernameBlock, usernameBox, passwordBlock, passwordBox, _loginButton, _registerButton, _errorBlock };
        }

        public void OnLoginFailed(string error)
        {
            _menu.Remove(_loginBlock);
            _menu.Add(_loginButton);
            _menu.Add(_registerButton);
            _errorBlock.Text = error;
            Reload();
        }
    }
}