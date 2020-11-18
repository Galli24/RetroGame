using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    public enum ClientMessageType
    {
        UNDEFINED,
        // Connect
        CONNECT,
        RECONNECT,
        // Lobby
        LOBBY_CREATE,
        LOBBY_JOIN,
        LOBBY_LEAVE
    }

    public enum MessageTarget
    {
        UNDEFINED,
        CONNECT,
        LOBBBY,
        GAME,
    }

    [ProtoContract(SkipConstructor = true)]
    // Connect
    [ProtoInclude(3, typeof(ClientConnectMessage))]
    [ProtoInclude(4, typeof(ClientReconnectMessage))]
    // Lobby
    [ProtoInclude(5, typeof(ClientLobbyCreateMessage))]
    [ProtoInclude(6, typeof(ClientLobbyJoinMessage))]
    [ProtoInclude(7, typeof(ClientLobbyLeaveMessage))]
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
