using LibNetworking.Models;
using System;
using System.Numerics;

namespace GameServer.Game
{
    class ServerBullet
    {
        private Bullet _bullet;
        public Vector2 Position;
        public bool ShouldDestroy;

        public ServerBullet(Bullet bullet, Vector2 position)
        {
            _bullet = bullet;
            Position = position;
        }

        public void Update(float fixedDeltaTime)
        {
            if (Position.X < 2000)
            {
                Position.X += fixedDeltaTime * Bullet.SPEED;
                _bullet.Position = Position;
            }
            else
                ShouldDestroy = true;
        }
    }
}
