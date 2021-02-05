using LibNetworking.Models;
using System;
using System.Numerics;

namespace GameServer.Game
{
    class ServerBullet
    {
        public Guid Id { get; private set; }
        public Vector2 Position;
        public bool ShouldDestroy;

        public ServerBullet(Guid id, Vector2 position)
        {
            Id = id;
            Position = position;
        }

        public void Update(float fixedDeltaTime)
        {
            if (Position.X < 2000)
                Position.X += fixedDeltaTime * Bullet.SPEED;
            else
                ShouldDestroy = true;
        }
    }
}
