using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientReconnectMessage : ClientMessage
    {
        [ProtoMember(1)]
        public string Token { get; private set; }
        [ProtoMember(2)]
        public string Id { get; private set; }
        [ProtoMember(3)]
        public string Username { get; private set; }

        public ClientReconnectMessage()
            : base(ClientMessageType.CONNECT, MessageTarget.CONNECT) { }

        public ClientReconnectMessage(string token, string id, string username)
            : base(ClientMessageType.RECONNECT, MessageTarget.CONNECT)
        {
            Token = token;
            Id = id;
            Username = username;
        }
    }
}
