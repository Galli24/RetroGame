using ProtoBuf;
using System.Net.Sockets;
using System.Numerics;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerGamePlayerPositionMessage : ServerMessage
    {
        [ProtoMember(1)]
        public string PlayerName { get; private set; }
        [ProtoMember(2)]
        public Vector2 Position { get; private set; }

        public ServerGamePlayerPositionMessage()
            : base(null, ServerMessageType.GAME_PLAYER_POSITION, MessageTarget.GAME) { }

        public ServerGamePlayerPositionMessage(Socket destination, string playerName, Vector2 position)
            : base(destination, ServerMessageType.GAME_PLAYER_POSITION, MessageTarget.GAME)
        {
            PlayerName = playerName;
            Position = position;
        }
    }
}
