using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientLobbyReadyMessage : ClientMessage
    {

        [ProtoMember(1)]
        public bool Ready { get; private set; }

        public ClientLobbyReadyMessage()
            : base(ClientMessageType.LOBBY_READY, MessageTarget.LOBBY) { }

        public ClientLobbyReadyMessage(bool ready)
            : base(ClientMessageType.LOBBY_READY, MessageTarget.LOBBY)
        {
            Ready = ready;
        }
    }
}
