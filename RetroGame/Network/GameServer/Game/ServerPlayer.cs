using LibNetworking.Models;
using System;
using System.Linq;
using System.Numerics;

namespace GameServer.Game
{
    class ServerPlayer : Player
    {
        public Guid Id;
        public GameManager _gameManager;

        public ServerPlayer(SocketState state, GameManager gameManager)
            : base(state)
        {
            Id = Guid.NewGuid();
            _gameManager = gameManager;
        }

        public void Update(float fixedDeltaTime)
        {
            var p = Vector2.Zero;
            var speed = Player.SPEED;
            foreach (var action in ActionStates.Where(action => action.Value))
            {
                switch (action.Key)
                {
                    case Actions.MOVE_LEFT:
                        p.X -= 1;
                        break;
                    case Actions.MOVE_RIGHT:
                        p.X += 1;
                        break;
                    case Actions.MOVE_DOWN:
                        p.Y -= 1;
                        break;
                    case Actions.MOVE_UP:
                        p.Y += 1;
                        break;
                    case Actions.BOOST:
                        speed = 1000;
                        break;
                    case Actions.SHOOT:
                        if (ShootCooldown <= 0)
                        {
                            ShootCooldown = SHOOT_COOLDOWN_TIME;
                            _gameManager.AddBullet(new Vector2(Position.X, Position.Y));
                        }
                        else
                            ShootCooldown -= fixedDeltaTime;
                        break;
                }
            }

            if (p != Vector2.Zero)
                Position += p * fixedDeltaTime * speed;
        }
    }
}
