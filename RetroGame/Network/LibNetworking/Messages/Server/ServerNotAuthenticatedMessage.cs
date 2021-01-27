using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerNotAuthenticatedMessage : ServerMessage
    {
        public ServerNotAuthenticatedMessage()
            : base (null, ServerMessageType.NOT_AUTHENTICATED, MessageTarget.CONNECT) { }

        public ServerNotAuthenticatedMessage(Socket destination)
            : base(destination, ServerMessageType.NOT_AUTHENTICATED, MessageTarget.CONNECT) { }
    }
}
