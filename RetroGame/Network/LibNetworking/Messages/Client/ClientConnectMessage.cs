using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public class ClientConnectMessage : ClientMessage
    {
        [ProtoMember(1)]
        public string Token { get; private set; }
        [ProtoMember(2)]
        public string Id { get; private set; }
        [ProtoMember(3)]
        public string Username { get; private set; }

        public ClientConnectMessage()
            : base(ClientMessageType.CONNECT, MessageTarget.CONNECT) { }

        public ClientConnectMessage(string token, string id, string username)
            : base(ClientMessageType.CONNECT, MessageTarget.CONNECT)
        {
            Token = token;
            Id = id;
            Username = username;
        }
    }
}
