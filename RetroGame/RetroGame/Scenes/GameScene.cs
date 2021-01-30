using LibNetworking.Models;
using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Timers;

namespace RetroGame.Scenes
{
    class GameScene : Scene
    {
        private enum Keys
        {
            Left = 263,
            Up = 265,
            Right = 262,
            Down = 264,
            Boost = 340,
        }

        private List<IMenu> _menu;
        public override IEnumerable<IMenu> Menu => _menu;
        public override IEnumerable<IGraphNode> Sprites => _players.Select(elt => elt.Value);

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        public Dictionary<string, AnimatedSprite> _players = new Dictionary<string, AnimatedSprite>();

        private Dictionary<Keys, bool> _keyStates = new Dictionary<Keys, bool>();

        private Dictionary<string, float> _lerpValues = new Dictionary<string, float>();

        private Window _win = RenderService.Instance.Window;

        private Timer _fixedLoopTimer;

        private TextBlock _fixedUpdates;

        public GameScene()
        {
            _win.OnKeyPressed += (k, m) => OnKeyAction(k, m, true);
            _win.OnKeyRelease += (k, m) => OnKeyAction(k, m, false);

            GameManager.Instance.OnPlayerUpdated += OnPlayerUpdated;

            _fixedLoopTimer = new Timer();
            _fixedLoopTimer.Elapsed += (_, __) => FixedUpdate();
            _fixedLoopTimer.Interval = GameManager.Instance.TickRateDeltaTime;
            _fixedLoopTimer.Start();
        }

        public void Stop()
        {
            _fixedLoopTimer.Stop();
        }

        private void OnPlayerUpdated(object sender, EventArgs e)
        {
            var player = sender as Player;
            _lerpValues[player.Name] = 0;
        }

        private void OnKeyAction(int key, int mods, bool pressed)
        {
            switch (key)
            {
                case (int)Keys.Left:
                case (int)Keys.Right:
                case (int)Keys.Down:
                case (int)Keys.Up:
                case (int)Keys.Boost:
                    if (!_keyStates.ContainsKey((Keys)key) || _keyStates[(Keys)key] != pressed)
                        NetworkManager.Instance.SendPlayerKeyState(ConvertKeyToAction((Keys)key), pressed);
                    _keyStates[(Keys)key] = pressed;
                    break;
            }
        }

        private Player.Actions ConvertKeyToAction(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    return Player.Actions.MOVE_LEFT;
                case Keys.Right:
                    return Player.Actions.MOVE_RIGHT;
                case Keys.Down:
                    return Player.Actions.MOVE_DOWN;
                case Keys.Up:
                    return Player.Actions.MOVE_UP;
                case Keys.Boost:
                    return Player.Actions.BOOST;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown key {key}");
            }
        }

        public override void BuildScene()
        {
            _players.Clear();
            foreach (var p in GameManager.Instance.Players)
                _players.Add(p.Key, new AnimatedSprite(new[] { @"C:\Users\jerem\Pictures\connor.png" }, 1000, Vector2.Zero, new Vector2(100)));

            _fixedUpdates = new TextBlock(RenderService.Instance.Window.Size, "", IMenu.Anchor.TopRight, FontManager.Instance["Roboto"], Vector2.One * 10);
            _menu = new List<IMenu>() { _fixedUpdates };
        }

        private void HandleKeys(float dt)
        {
            var p = new Vector2(0);
            var speed = Player.SPEED;
            foreach (var elt in _keyStates.Where(elt => elt.Value))
            {
                switch (elt.Key)
                {
                    case Keys.Left:
                        p.X -= 1;
                        break;
                    case Keys.Right:
                        p.X += 1;
                        break;
                    case Keys.Down:
                        p.Y -= 1;
                        break;
                    case Keys.Up:
                        p.Y += 1;
                        break;
                    case Keys.Boost:
                        speed = 1000;
                        break;
                }
            }

            if (p != Vector2.Zero)
                GameManager.Instance.Players[UserManager.Instance.Username].Position += p * dt * speed;
        }

        public void Update(float deltaTime)
        {
            _players[UserManager.Instance.Username].Position = GameManager.Instance.Players[UserManager.Instance.Username].Position;

            foreach (var lv in _lerpValues.Keys)
                _lerpValues[lv] += deltaTime / GameManager.Instance.TickRateDeltaTime;

            foreach (var p in _players)
            {
                if (p.Key != UserManager.Instance.Username)
                    p.Value.Position = Vector2.Lerp(
                        GameManager.Instance.Players[p.Key].LastRenderedPosition,
                        GameManager.Instance.Players[p.Key].Position,
                        Math.Clamp(_lerpValues[p.Key], 0, 1));
            }
        }

        private void FixedUpdate()
        {
            var fixedDeltaTime = GameManager.Instance.TickRateDeltaTime;
            HandleKeys(fixedDeltaTime);
        }
    }
}
