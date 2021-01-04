using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientLobbyLeaveMessage : ClientMessage
    {
        public ClientLobbyLeaveMessage()
            : base(ClientMessageType.LOBBY_LEAVE, MessageTarget.LOBBY) { }
    }
}
