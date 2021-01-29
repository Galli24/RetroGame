using RenderEngine;
using RetroGame.Model;
using RetroGame.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

        public override IEnumerable<IMenu> Menu => Array.Empty<IMenu>();
        public override IEnumerable<IGraphNode> Sprites => _players.Select(elt => elt.Value);

        public override bool RequireClearOnLoad => true;

        public override bool RequireClearOnExit => true;

        public Dictionary<string, AnimatedSprite> _players = new Dictionary<string, AnimatedSprite>();

        private Dictionary<Keys, bool> _keyStates = new Dictionary<Keys, bool>();


        private Window _win = RenderService.Instance.Window;


        public GameScene()
        {
            _win.OnKeyPressed += (k, m) => OnKeyAction(k, m, true);
            _win.OnKeyRelease += (k, m) => OnKeyAction(k, m, false);
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
                    _keyStates[(Keys)key] = pressed;
                    break;
            }
        }


        public override void BuildScene()
        {
            _players.Clear();
            foreach (var p in GameManager.Instance.Players)
                _players.Add(p.Key, new AnimatedSprite(new[] { @"C:\Users\jerem\Pictures\connor.png" }, 1000, Vector2.Zero, new Vector2(100)));
        }

        private void HandleKeys(float dt)
        {
            var p = new Vector2(0);
            var speed = 100f;
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
            //_players[UserManager.Instance.Username].Position += p * dt * speed;
            if (p != Vector2.Zero)
            {
                var position = GameManager.Instance.Players[UserManager.Instance.Username].Position += p * dt * speed;
                NetworkManager.Instance.SendPlayerPosition(position);
            }
        }

        public void Update(float deltaTime)
        {
            HandleKeys(deltaTime);

            foreach (var p in _players)
                p.Value.Position = GameManager.Instance.Players[p.Key].Position;
        }
    }
}
