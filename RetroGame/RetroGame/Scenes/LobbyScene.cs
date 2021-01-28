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

        public override IEnumerable<IGraphNode> Sprites => Array.Empty<IGraphNode>();
        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        public LobbyScene() : base() { }

        private TextBlock[] _players = new TextBlock[0];


        public override void BuildScene()
        {
            var sc = RenderService.Instance;

            var headerYPosition = (sc.Window.Size.Y / 2) + .4f * sc.Window.Size.Y;
            float playerYPosition(int i) => (sc.Window.Size.Y * .75f) - i * 70;
            string readyButtonText() => LobbyManager.Instance.IsReady ? "Not ready" : "Ready";
            Vector4 playerReadyColor(int index) => LobbyManager.Instance.PlayerList.ElementAt(index).Value.IsReady ? new Vector4(.8f, .8f, .8f, 1) : new Vector4(.25f, .25f, .25f, 1);
            string playerName(int index) => LobbyManager.Instance.PlayerList.ElementAt(index).Value.IsHost ? "* " + LobbyManager.Instance.PlayerList.ElementAt(index).Key : LobbyManager.Instance.PlayerList.ElementAt(index).Key;

            var title = new TextBlock(new Vector2(sc.Window.Size.X / 2, headerYPosition),
                LobbyManager.Instance.LobbyName,
                IMenu.Anchor.Center, FontManager.Instance["Roboto", 100], Vector2.One * 10);

            _players = Enumerable.Range(0, LobbyManager.Instance.PlayerList.Count).Select(index =>
                new TextBlock(new Vector2(30, playerYPosition(index)), playerName(index), IMenu.Anchor.Left, FontManager.Instance["Roboto"], Vector2.One * 10)
            ).ToArray();

            for (int i = 0; i < _players.Length; i++)
                _players[i].BorderColor = playerReadyColor(i);


            var leaveBt = new Button(new Vector2(30, 30), "Leave", IMenu.Anchor.BottomLeft, FontManager.Instance["Roboto", 50], Vector2.Zero);
            leaveBt.Padding = new Vector2((sc.Window.Size.X / 4 - leaveBt.EvaluatedSize.X / 2) - 30, 10);
            if (!LobbyManager.Instance.IsStarting)
                leaveBt.OnMousePress += (_, __, ___) => LobbyManager.Instance.LeaveLobby();

            var readyBt = new Button(new Vector2(sc.Window.Size.X - 30, 30), readyButtonText(), IMenu.Anchor.BottomRight, FontManager.Instance["Roboto", 50], Vector2.Zero);
            readyBt.Padding = new Vector2((sc.Window.Size.X / 4 - readyBt.EvaluatedSize.X / 2) - 30, 10);
            readyBt.OnMousePress += (_, __, ___) => LobbyManager.Instance.ToggleReady();

            var startButton = new Button(new Vector2(sc.Window.Size.X - 30, 30), LobbyManager.Instance.IsStarting ? "Starting..." : "Start Game", IMenu.Anchor.BottomRight, FontManager.Instance["Roboto", 50], Vector2.Zero);
            startButton.Padding = new Vector2((sc.Window.Size.X / 4 - startButton.EvaluatedSize.X / 2) - 30, 10);
            if (!LobbyManager.Instance.IsStarting)
                startButton.OnMousePress += (_, __, ___) => LobbyManager.Instance.StartGame();


            var list = new List<IMenu> { title, leaveBt };
            list.Add(LobbyManager.Instance.AllPlayersReady() && LobbyManager.Instance.IsHost ? startButton : readyBt);

            _menu = _players.Concat(list).ToList();
        }

    }
}