using LibNetworking.Messages.Server;
using LibNetworking.Models;
using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using RetroGame.Utils;
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
            Shoot = 32
        }

        #region Scene

        public override bool RequireClearOnLoad => true;
        public override bool RequireClearOnExit => true;
        public override IEnumerable<IMenu> Menu => _menu;
        public override IEnumerable<IGraphNode> Sprites => _players.Select(elt => elt.Value);

        #endregion

        private List<IMenu> _menu;
        public Dictionary<string, AnimatedSprite> _players = new Dictionary<string, AnimatedSprite>();
        private Dictionary<Player.Actions, bool> _actionStates;
        private Window _win = RenderService.Instance.Window;

        private ObjectPool<AnimatedSprite> _bulletPool;

        private Dictionary<Guid, AnimatedSprite> _bullets = new Dictionary<Guid, AnimatedSprite>();

        private List<Guid> _bulletsBeingAdded = new List<Guid>();

        #region Menu

        private TextBlock _fixedUpdates;
        private Timer _fixedLoopTimer;

        #endregion

        public GameScene()
        {
            _bulletPool = new ObjectPool<AnimatedSprite>(() => new AnimatedSprite(new List<string> { @"C:\Users\jerem\Pictures\PixelArt\BowserBlue.png" }, 1000, Vector2.Zero, new Vector2(32, 48)));

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
                case (int)Keys.Shoot:
                    Player.Actions action = ConvertKeyToAction((Keys)key);
                    _actionStates[action] = pressed;
                    break;
            }
        }

        private static Player.Actions ConvertKeyToAction(Keys key)
        {
            return key switch
            {
                Keys.Left => Player.Actions.MOVE_LEFT,
                Keys.Right => Player.Actions.MOVE_RIGHT,
                Keys.Down => Player.Actions.MOVE_DOWN,
                Keys.Up => Player.Actions.MOVE_UP,
                Keys.Boost => Player.Actions.BOOST,
                Keys.Shoot => Player.Actions.SHOOT,
                _ => throw new ArgumentOutOfRangeException($"Unknown key {key}"),
            };
        }

        public override void BuildScene()
        {
            _players.Clear();
            foreach (var p in GameManager.Instance.Players)
            {
                if (p.Key == "server difference")
                    _players.Add(p.Key, new AnimatedSprite(new[] { @"C:\Users\jerem\Pictures\mlg_glasses.png" }, 1000, Vector2.Zero, new Vector2(100)));
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

            var clampedPos = Vector2.Clamp(GameManager.Instance.Players[UserManager.Instance.Username].Position + p * dt * speed, Vector2.Zero, new Vector2(1920, 1080) - new Vector2(100));
            GameManager.Instance.Players[UserManager.Instance.Username].Position = clampedPos;
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

            foreach (var bullet in GameManager.Instance.Bullets.ToArray())
            {
                if (!_bullets.ContainsKey(bullet.Key) && !_bulletsBeingAdded.Contains(bullet.Key))
                {
                    _bulletsBeingAdded.Add(bullet.Key);
                    RenderService.Instance.DoInRenderThread(() =>
                    {
                        _bullets.Add(bullet.Key, _bulletPool.Spawn(bullet.Value.Position));
                        _bulletsBeingAdded.Remove(bullet.Key);
                    });
                }
                else if (!_bulletsBeingAdded.Contains(bullet.Key))
                {
                    if (bullet.Value.LerpElapsed < bullet.Value.LerpDuration)
                    {
                        _bullets[bullet.Key].Position = Vector2.Lerp(
                            bullet.Value.LastRenderedPosition,
                            bullet.Value.Position,
                            bullet.Value.LerpElapsed / bullet.Value.LerpDuration);
                        bullet.Value.LerpElapsed += fixedDeltaTime;
                    }
                    else
                        _bullets[bullet.Key].Position = bullet.Value.Position;
                }
            }

            var toRemove = _bullets.ToArray().Where(elt => GameManager.Instance.Bullets.ToArray().All(e => e.Key != elt.Key));
            foreach (var rm in toRemove)
            {
                RenderService.Instance.DoInRenderThread(() =>
                {
                    _bulletPool.Despawn(rm.Value);
                    _bullets.Remove(rm.Key);
                });
            }




            GameManager.Instance.PlayerBufferedHistory[GameManager.Instance.CurrentClientTick] = GameManager.Instance.Players[UserManager.Instance.Username].CloneForBuffer();
            GameManager.Instance.CurrentClientTick++;
        }
    }
}
