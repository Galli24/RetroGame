using ProtoBuf;

namespace LibNetworking.Messages.Server
{
    public enum ServerMessageType
    {
        UNDEFINED,
    }

    [ProtoContract]
    public abstract class ServerMessage : Message
    {
        [ProtoMember(1)]
        public ServerMessageType ServerMessageType { get; private set; }

        protected ServerMessage(ServerMessageType serverMessageType)
            : base(MessageType.SERVER)
        {
            ServerMessageType = serverMessageType;
        }
    }
}
