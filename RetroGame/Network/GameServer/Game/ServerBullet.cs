using LibNetworking.Models;
using System;
using System.Numerics;

namespace GameServer.Game
{
    class ServerBullet : Bullet
    {
        public bool ShouldDestroy;

        public ServerBullet(Guid id, Vector2 position)
            : base(id, position) { }

        public void Update(float fixedDeltaTime)
        {
            if (Position.X < 2000)
                Position = new Vector2(Position.X + fixedDeltaTime * SPEED, Position.Y);
            else
                ShouldDestroy = true;
        }
    }
}
