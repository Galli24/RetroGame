using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    public enum ClientMessageType
    {
        UNDEFINED,
        // Connect
        CONNECT,
        // Lobby
        LOBBY_CREATE,
        LOBBY_JOIN,
        LOBBY_READY,
        LOBBY_START,
        LOBBY_LEAVE
    }

    public enum MessageTarget
    {
        UNDEFINED,
        CONNECT,
        LOBBY,
        GAME,
    }

    [ProtoContract(SkipConstructor = true)]
    // Connect
    [ProtoInclude(3, typeof(ClientConnectMessage))]
    // Lobby
    [ProtoInclude(4, typeof(ClientLobbyCreateMessage))]
    [ProtoInclude(5, typeof(ClientLobbyJoinMessage))]
    [ProtoInclude(6, typeof(ClientLobbyReadyMessage))]
    [ProtoInclude(7, typeof(ClientLobbyStartMessage))]
    [ProtoInclude(8, typeof(ClientLobbyLeaveMessage))]
    public abstract class ClientMessage : Message
    {
        [ProtoMember(1)]
        public ClientMessageType ClientMessageType { get; private set; }
        [ProtoMember(2)]
        public MessageTarget MessageTarget { get; private set; }

        protected ClientMessage(ClientMessageType clientMessageType, MessageTarget messageTarget)
            : base(MessageType.CLIENT)
        {
            ClientMessageType = clientMessageType;
            MessageTarget = messageTarget;
        }
    }
}
