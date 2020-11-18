using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerConnectedMessage : ServerMessage
    {
        [ProtoMember(1)]
        public bool Authorized { get; private set; }
        [ProtoMember(2)]
        public string Reason { get; private set; }

        public ServerConnectedMessage()
            : base (null, ServerMessageType.CONNECTED) { }

        public ServerConnectedMessage(Socket destination, bool authorized, string reason)
            : base(destination, ServerMessageType.CONNECTED)
        {
            Authorized = authorized;
            Reason = reason;
        }
    }
}
