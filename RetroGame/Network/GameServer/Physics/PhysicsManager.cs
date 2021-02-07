using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GameServer.Physics
{
    internal class PhysicsManager
    {
        private readonly Dictionary<Guid, Vector2> _sizes = new Dictionary<Guid, Vector2>();

        public void RegisterItem(Guid id, Vector2 size) => _sizes.Add(id, size);

        public void UnregisterItem(Guid id) => _sizes.Remove(id);

        public Dictionary<Guid, Guid> ComputeCollisions(Dictionary<Guid, Vector2> playersGroup, Dictionary<Guid, Vector2> enemiesGroup)
        {
            var output = new Dictionary<Guid, Guid>();
            var playersArray = playersGroup.ToArray();
            var ennemiesArray = enemiesGroup.ToArray();

            foreach (var item1 in playersArray)
            {
                var size1 = _sizes[item1.Key];

                foreach (var item2 in ennemiesArray)
                {
                    var size2 = _sizes[item2.Key];

                    if (DoCollide(item1.Value, size1, item2.Value, size2))
                        output.TryAdd(item1.Key, item2.Key);
                }
            }

            return output;
        }
        public static bool DoCollide(Vector2 minCorner1, Vector2 size1, Vector2 minCorner2, Vector2 size2)
        {
            var maxCorner1 = minCorner1 + size1;
            var maxCorner2 = minCorner2 + size2;

            return
                minCorner1.X > minCorner2.X && minCorner1.X < maxCorner2.X && minCorner1.Y > minCorner2.Y && minCorner1.Y < maxCorner2.Y ||
                minCorner2.X > minCorner1.X && minCorner2.X < maxCorner1.X && minCorner2.Y > minCorner1.Y && minCorner2.Y < maxCorner1.Y;
        }

    }
}
