using ProtoBuf;
using System.Collections.Generic;
using System.Numerics;

namespace LibNetworking.Models
{
    [ProtoContract(SkipConstructor = true)]
    public class Player
    {
        #region Action handling

        public enum Actions
        {
            MOVE_LEFT,
            MOVE_UP,
            MOVE_RIGHT,
            MOVE_DOWN,
            BOOST
        }

        [ProtoIgnore]
        public readonly Dictionary<Actions, bool> ActionStates = new Dictionary<Actions, bool>();

        [ProtoIgnore]
        public const float SPEED = 100;

        #endregion

        #region Members

        [ProtoIgnore]
        public readonly SocketState State;
        [ProtoIgnore]
        private bool _isDirty;
        [ProtoIgnore]
        public bool IsDirty
        {
            get => _isDirty;
            set => _isDirty = value;
        }

        [ProtoMember(1)]
        public string Name { get; private set; }
        [ProtoIgnore]
        public bool IsHost;
        [ProtoIgnore]
        public bool IsReady;

        [ProtoMember(2)]
        private float _x;
        [ProtoMember(3)]
        private float _y;

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
                _isDirty = true;
            }
        }

        #endregion

        #region Constructors

        // Parameters-less constructor for ProtoBuf

        public Player() { }

        // Server constructors

        public Player(SocketState state, bool isHost)
        {
            State = state;
            Name = state.Username;
            IsHost = isHost;
        }

        public Player(SocketState state)
        {
            State = state;
            Name = state.Username;
            Position = Vector2.Zero;
        }

        // Client constructors

        public Player(string name, bool isHost, bool isReady)
        {
            Name = name;
            IsHost = isHost;
            IsReady = isReady;
        }

        #endregion
    }
}
