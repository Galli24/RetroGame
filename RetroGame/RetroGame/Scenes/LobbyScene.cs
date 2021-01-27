using Microsoft.VisualBasic.CompilerServices;
using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private TextBlock[] _players = new TextBlock[0];

        private bool _isReady = false;

        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var headerYPosition = (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y;
            float playerYPosition(int i) => (sc.Window.Size.Y * .75f) - i * 70;
            string readyButtonText() => _isReady ? "Not ready" : "Ready";
            Vector4 playerReadyColor(int index) => LobbyManager.Instance.PlayerList.ElementAt(index).Value ? new Vector4(.8f, .8f, .8f, 1) : new Vector4(.25f, .25f, .25f, 1);


            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, headerYPosition),
                LobbyManager.Instance.LobbyName,
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 100], Vector2.One * 10);

            _players = Enumerable.Range(0, LobbyManager.Instance.PlayerList.Count).Select(index =>
                new TextBlock(new Vector2(30, playerYPosition(index)), $"Player {index}", IMenu.Anchor.Left, FontManager.Instance["Roboto"], Vector2.One * 10)
            ).ToArray();

            for (int i = 0; i < _players.Length; i++)
                _players[i].BorderColor = playerReadyColor(i);


            var leaveBt = new Button(new Vector2(30, 30), "Leave", IMenu.Anchor.BottomLeft, FontManager.Instance["Roboto", 50], Vector2.Zero);
            leaveBt.Padding = new Vector2((sc.Window.Size.X / 4 - leaveBt.EvaluatedSize.X / 2) - 30, 10);
            leaveBt.OnMousePress += (_, __, ___) =>
            {
                Trace.Write("CIAO");
            };

            var readyBt = new Button(new Vector2(sc.Window.Size.X - 30, 30), readyButtonText(), IMenu.Anchor.BottomRight, FontManager.Instance["Roboto", 50], Vector2.Zero);
            readyBt.Padding = new Vector2((sc.Window.Size.X / 4 - readyBt.EvaluatedSize.X / 2) - 30, 10);
            readyBt.OnMousePress += (_, __, ___) =>
            {
                _isReady = !_isReady;
                readyBt.Text = readyButtonText();
                readyBt.Padding = new Vector2((sc.Window.Size.X / 4 - readyBt.EvaluatedSize.X / 2) - 30, 10);
                _players[0].BorderColor = _isReady ? new Vector4(.8f, .8f, .8f, 1) : new Vector4(.25f, .25f, .25f, 1);
                Trace.WriteLine("MANU CIAO");
            };

            var startButton = new Button(new Vector2(sc.Window.Size.X - 30, 30), "Start Game", IMenu.Anchor.BottomRight, FontManager.Instance["Roboto", 50], Vector2.Zero);
            readyBt.Padding = new Vector2((sc.Window.Size.X / 4 - readyBt.EvaluatedSize.X / 2) - 30, 10);
            readyBt.OnMousePress += (_, __, ___) =>
            {
                Trace.WriteLine("Start");
            };


            _menu = _players.Concat(new List<IMenu> { title, leaveBt, readyBt }).ToList();
        }

    }
}