using ProtoBuf;
using System.Numerics;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientGamePlayerPositionMessage : ClientMessage
    {
        [ProtoMember(1)]
        private readonly float _x;
        [ProtoMember(2)]
        private readonly float _y;

        [ProtoIgnore]
        public Vector2 Position => new Vector2(_x, _y);

        public ClientGamePlayerPositionMessage()
            : base(ClientMessageType.GAME_PLAYER_POSITION, MessageTarget.GAME) { }

        public ClientGamePlayerPositionMessage(Vector2 position)
            : base(ClientMessageType.GAME_PLAYER_POSITION, MessageTarget.GAME)
        {
            _x = position.X;
            _y = position.Y;
        }
    }
}
