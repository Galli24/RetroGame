using LibNetworking.Messages.Server;
using LibNetworking.Models;
using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private Dictionary<Player.Actions, bool> _actionStates;

        private Window _win = RenderService.Instance.Window;

        private Timer _fixedLoopTimer;

        private TextBlock _fixedUpdates;

        public GameScene()
        {
            _actionStates = Enum.GetValues(typeof(Player.Actions))
                .Cast<Player.Actions>()
                .ToDictionary(key => key, state => false);

            _win.OnKeyPressed += (k, m) => OnKeyAction(k, m, true);
            _win.OnKeyRelease += (k, m) => OnKeyAction(k, m, false);

            _fixedLoopTimer = new Timer();
            _fixedLoopTimer.Elapsed += (_, __) => FixedUpdate();
            _fixedLoopTimer.Interval = GameManager.Instance.TickRateDeltaTime * 1000;
            _fixedLoopTimer.Start();
        }

        public void Stop()
        {
            _fixedLoopTimer.Stop();
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
                    Player.Actions action = ConvertKeyToAction((Keys)key);
                    _actionStates[action] = pressed;
                    break;
            }
        }

        private Player.Actions ConvertKeyToAction(Keys key)
        {
            return key switch
            {
                Keys.Left => Player.Actions.MOVE_LEFT,
                Keys.Right => Player.Actions.MOVE_RIGHT,
                Keys.Down => Player.Actions.MOVE_DOWN,
                Keys.Up => Player.Actions.MOVE_UP,
                Keys.Boost => Player.Actions.BOOST,
                _ => throw new ArgumentOutOfRangeException($"Unknown key {key}"),
            };
        }

        public override void BuildScene()
        {
            _players.Clear();
            foreach (var p in GameManager.Instance.Players)
            {
                if (p.Key == "server difference")
                    _players.Add(p.Key, new AnimatedSprite(new[] { @"C:\Users\jerem\Pictures\mlg_connor.png" }, 1000, Vector2.Zero, new Vector2(100)));
                else
                    _players.Add(p.Key, new AnimatedSprite(new[] { @"C:\Users\jerem\Pictures\connor.png" }, 1000, Vector2.Zero, new Vector2(100)));
            }

            _fixedUpdates = new TextBlock(RenderService.Instance.Window.Size, "", IMenu.Anchor.TopRight, FontManager.Instance["Roboto"], Vector2.One * 10);
            _menu = new List<IMenu>() { _fixedUpdates };
        }

        private void HandleKeys(float dt)
        {
            var p = Vector2.Zero;
            var speed = Player.SPEED;

            // Make sure to use the same inputs that are sent
            var actionStates = _actionStates.ToDictionary(kv => kv.Key, kv => kv.Value);
            GameManager.Instance.Players[UserManager.Instance.Username].ActionStates = actionStates;
            NetworkManager.Instance.SendPlayerActionStates(GameManager.Instance.CurrentClientTick, actionStates);

            foreach (var elt in actionStates.Where(elt => elt.Value))
            {
                switch (elt.Key)
                {
                    case Player.Actions.MOVE_LEFT:
                        p.X -= 1;
                        break;
                    case Player.Actions.MOVE_RIGHT:
                        p.X += 1;
                        break;
                    case Player.Actions.MOVE_DOWN:
                        p.Y -= 1;
                        break;
                    case Player.Actions.MOVE_UP:
                        p.Y += 1;
                        break;
                    case Player.Actions.BOOST:
                        speed = 1000;
                        break;
                }
            }

            GameManager.Instance.Players[UserManager.Instance.Username].Position += p * dt * speed;
            if (_players.ContainsKey(UserManager.Instance.Username))
                _players[UserManager.Instance.Username].Position = GameManager.Instance.Players[UserManager.Instance.Username].Position;
        }

        public void Update(float deltaTime)
        {
            //_players[UserManager.Instance.Username].Position = GameManager.Instance.Players[UserManager.Instance.Username].Position;
        }

        private void FixedUpdate()
        {
            var fixedDeltaTime = GameManager.Instance.TickRateDeltaTime;
            HandleKeys(fixedDeltaTime);

            foreach (var p in _players)
            {
                if (p.Key != UserManager.Instance.Username && GameManager.Instance.Players.ContainsKey(p.Key))
                {
                    if (GameManager.Instance.Players[p.Key].LerpElapsed < GameManager.Instance.Players[p.Key].LerpDuration)
                    {
                        p.Value.Position = Vector2.Lerp(
                            GameManager.Instance.Players[p.Key].LastRenderedPosition,
                            GameManager.Instance.Players[p.Key].Position,
                            GameManager.Instance.Players[p.Key].LerpElapsed / GameManager.Instance.Players[p.Key].LerpDuration);
                        GameManager.Instance.Players[p.Key].LerpElapsed += fixedDeltaTime;
                    }
                    else
                    {
                        p.Value.Position = GameManager.Instance.Players[p.Key].Position;
                    }
                }
            }

            GameManager.Instance.PlayerBufferedHistory[GameManager.Instance.CurrentClientTick] = GameManager.Instance.Players[UserManager.Instance.Username].CloneForBuffer();
            GameManager.Instance.CurrentClientTick++;
        }
    }
}
