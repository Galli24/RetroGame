using ProtoBuf;
using System.Numerics;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientGamePlayerPositionMessage : ClientMessage
    {
        [ProtoMember(1)]
        public Vector2 Position { get; private set; }

        public ClientGamePlayerPositionMessage()
            : base(ClientMessageType.GAME_PLAYER_POSITION, MessageTarget.GAME) { }

        public ClientGamePlayerPositionMessage(Vector2 position)
            : base(ClientMessageType.GAME_PLAYER_POSITION, MessageTarget.GAME)
        {
            Position = position;
        }
    }
}
