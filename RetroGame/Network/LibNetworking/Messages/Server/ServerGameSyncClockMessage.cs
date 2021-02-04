using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerGameSyncClockMessage : ServerMessage
    {
        [ProtoMember(1)]
        public long RequestedClock { get; private set; }

        public ServerGameSyncClockMessage()
            : base(null, ServerMessageType.GAME_SYNC_CLOCK, MessageTarget.GAME) { }

        public ServerGameSyncClockMessage(Socket destination, long requestedClock)
            : base(destination, ServerMessageType.GAME_SYNC_CLOCK, MessageTarget.GAME)
        {
            RequestedClock = requestedClock;
        }
    }
}
