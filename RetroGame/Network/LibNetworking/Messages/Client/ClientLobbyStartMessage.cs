using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientLobbyStartMessage : ClientMessage
    {
        public ClientLobbyStartMessage()
            : base(ClientMessageType.LOBBY_START, MessageTarget.LOBBY) { }
    }
}
