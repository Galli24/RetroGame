using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerLobbyCreatedMessage : ServerMessage
    {
        [ProtoMember(1)]
        public bool HasJoined { get; private set; }
        [ProtoMember(2)]
        public string Reason { get; private set; }
        [ProtoMember(3)]
        public string Name { get; private set; }
        [ProtoMember(4)]
        public bool HasPassword { get; private set; }
        [ProtoMember(4)]
        public ushort MaxSlots { get; private set; }

        public ServerLobbyCreatedMessage()
            : base (null, ServerMessageType.LOBBY_CREATED) { }

        public ServerLobbyCreatedMessage(Socket destination, bool hasJoined, string reason, string name, bool hasPassword, ushort maxSlots)
            : base(destination, ServerMessageType.LOBBY_CREATED)
        {
            HasJoined = hasJoined;
            Reason = reason;
            Name = name;
            HasPassword = hasPassword;
            MaxSlots = maxSlots;
        }
    }
}
