using ProtoBuf;
using System.Collections.Generic;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerLobbyJoinedMessage : ServerMessage
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
        [ProtoMember(5)]
        public List<string> PlayerList { get; private set; }

        public ServerLobbyJoinedMessage()
            : base (null, ServerMessageType.LOBBY_JOINED, MessageTarget.LOBBY) { }

        public ServerLobbyJoinedMessage(
            Socket destination, bool hasJoined,
            string reason, string name,
            bool hasPassword, ushort maxSlots,
            List<string> playerList)
            : base(destination, ServerMessageType.LOBBY_JOINED, MessageTarget.LOBBY)
        {
            HasJoined = hasJoined;
            Reason = reason;
            Name = name;
            HasPassword = hasPassword;
            MaxSlots = maxSlots;
            PlayerList = playerList;
        }
    }
}
