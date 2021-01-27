using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
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

        private readonly RegisterScene _registerScene;

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

            var loginButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Login",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var loginBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .16f * sc.Window.Size.Y),
                "Logging in...",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                BorderColor = new Vector4(1, 1, 1, 1)
            };

            var registerButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .24f * sc.Window.Size.Y),
                "Register",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);

            var errorBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .32f * sc.Window.Size.Y),
                "",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                TextColor = new Vector4(1, 0, 0, 1)
            };

            loginButton.OnMousePress += async(_, __, ___) =>
            {
                loginButton.BorderColor = new Vector4(.5f, .5f, .5f, 1);
                _menu.Remove(loginButton);
                _menu.Remove(registerButton);
                _menu.Add(loginBlock);
                errorBlock.Text = "";
                Reload();
                var result = await NetworkManager.Instance.Connect(usernameBox.Text, passwordBox.Text);

                if (!string.IsNullOrEmpty(result))
                {
                    if (result != "Connected")
                    {
                        _menu.Remove(loginBlock);
                        _menu.Add(loginButton);
                        _menu.Add(registerButton);
                        errorBlock.Text = result;
                        Reload();
                    } else
                    {
                        // SceneManager.Instance.LoadScene();
                    }
                }
            };

            registerButton.OnMousePress += (_, __, ___) =>
            {
                SceneManager.Instance.LoadScene(_registerScene);
            };

            _menu = new List<IMenu> { title, usernameBlock, usernameBox, passwordBlock, passwordBox, loginButton, registerButton, errorBlock };
        }

    }
}