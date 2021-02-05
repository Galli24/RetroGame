using ProtoBuf;
using System;
using System.Numerics;

namespace LibNetworking.Models
{
    [ProtoContract(SkipConstructor = true)]
    public class Bullet
    {
        #region Members

        [ProtoIgnore]
        public readonly SocketState State;

        [ProtoMember(1)]
        public Guid Id { get; private set; }

        [ProtoMember(2)]
        private float _x;
        [ProtoMember(3)]
        private float _y;

        [ProtoIgnore]
        public float LerpElapsed;
        [ProtoIgnore]
        public float LerpDuration = 0.5f;
        [ProtoIgnore]
        public Vector2 LastRenderedPosition;
        [ProtoIgnore]
        public Vector2 Position
        {
            get => new Vector2(_x, _y);
            set
            {
                _x = value.X;
                _y = value.Y;
            }
        }

        [ProtoIgnore]
        public const float SPEED = 1000;

        #endregion

        #region Constructors

        // Parameters-less constructor for ProtoBuf

        public Bullet() { }

        // Server constructors

        public Bullet(Guid id, Vector2 position)
        {
            Id = id;
            Position = position;
        }

        #endregion
    }
}
