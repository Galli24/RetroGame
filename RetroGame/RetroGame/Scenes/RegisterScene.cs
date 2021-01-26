using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System.Collections.Generic;
using System.Numerics;

namespace RetroGame.Scenes
{
    class RegisterScene : Scene
    {
        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        private readonly LoginScene _loginScene;

        public RegisterScene(LoginScene loginScene) : base()
        {
            _loginScene = loginScene;
        }


        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y),
                "RetroGame",
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 100], Vector2.One * 10);

            var emailBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .26f * sc.Window.Size.Y),
                "Email",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var emailBox = new TextBox(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .2f * sc.Window.Size.Y),
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 28], Vector2.One * 10, 250);

            var usernameBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .1f * sc.Window.Size.Y),
                "Username",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var usernameBox = new TextBox(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) + .04f * sc.Window.Size.Y),
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 28], Vector2.One * 10, 250);

            var passwordBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .06f * sc.Window.Size.Y),
                "Password",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var passwordBox = new TextBox(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .12f * sc.Window.Size.Y),
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 28], Vector2.One * 10, 250);

            var registerButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .22f * sc.Window.Size.Y),
                "Register",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);
            var registerBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .22f * sc.Window.Size.Y),
                "Signing up...",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                BorderColor = new Vector4(1, 1, 1, 1)
            };

            var backButton = new Button(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .3f * sc.Window.Size.Y),
                "Back",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10);

            var errorBlock = new TextBlock(new Vector2(sc.Window.Size.X / 2, (sc.Window.Size.Y / 2) - .38f * sc.Window.Size.Y),
                "",
                IMenu.Anchor.Center, FontManager.Instance["Roboto"], Vector2.One * 10)
            {
                TextColor = new Vector4(1, 0, 0, 1)
            };

            registerButton.OnMousePress += async(_, __, ___) =>
            {
                registerButton.BorderColor = new Vector4(.5f, .5f, .5f, 1);
                _menu.Remove(registerButton);
                _menu.Remove(backButton);
                _menu.Add(registerBlock);
                errorBlock.Text = "";
                Reload();
                var result = await NetworkManager.Instance.Register(emailBox.Text, usernameBox.Text, passwordBox.Text);

                if (!string.IsNullOrEmpty(result))
                {
                    if (result != "Registered")
                    {
                        _menu.Remove(registerBlock);
                        _menu.Add(registerButton);
                        _menu.Add(backButton);
                        registerButton.Text = "Register";
                        errorBlock.Text = result;
                        Reload();
                    }
                    else
                    {
                        _menu.Remove(registerBlock);
                        _menu.Add(backButton);
                        errorBlock.TextColor = new Vector4(0, 1, 0, 1);
                        errorBlock.Text = "Registration successful";
                        Reload();
                    }
                }
            };

            backButton.OnMousePress += (_, __, ___) =>
            {
                SceneManager.Instance.LoadScene(_loginScene);
            };

            _menu = new List<IMenu> { title, emailBlock, emailBox, usernameBlock, usernameBox, passwordBlock, passwordBox, registerButton, backButton, errorBlock };
        }

    }
}