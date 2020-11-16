using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    public enum ServerMessageType
    {
        UNDEFINED,
        CONNECT,
    }

    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude(2, typeof(ServerConnectMessage))]
    public abstract class ServerMessage : Message
    {
        [ProtoIgnore]
        public Socket Destination { get; private set; }

        [ProtoMember(1)]
        public ServerMessageType ServerMessageType { get; private set; }

        protected ServerMessage(Socket destination, ServerMessageType serverMessageType)
            : base(MessageType.SERVER)
        {
            Destination = destination;
            ServerMessageType = serverMessageType;
        }
    }
}
