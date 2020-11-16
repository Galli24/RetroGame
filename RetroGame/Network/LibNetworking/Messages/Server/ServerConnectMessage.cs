using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public class ServerConnectMessage : ServerMessage
    {
        [ProtoMember(1)]
        public bool Authorized { get; private set; }
        [ProtoMember(2)]
        public string Reason { get; private set; }

        public ServerConnectMessage()
            : base (null, ServerMessageType.CONNECT) { }

        public ServerConnectMessage(Socket destination, bool authorized, string reason)
            : base(destination, ServerMessageType.CONNECT)
        {
            Authorized = authorized;
            Reason = reason;
        }
    }
}
