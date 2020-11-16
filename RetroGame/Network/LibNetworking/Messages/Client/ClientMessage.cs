using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    public enum ClientMessageType
    {
        UNDEFINED,
        CONNECT,
    }

    public enum MessageTarget
    {
        UNDEFINED,
        CONNECT,
        LOBBBY,
        GAME,
    }

    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude(3, typeof(ClientConnectMessage))]
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
