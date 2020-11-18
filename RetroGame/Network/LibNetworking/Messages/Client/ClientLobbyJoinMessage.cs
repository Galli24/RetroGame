using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientLobbyJoinMessage : ClientMessage
    {
        [ProtoMember(1)]
        public string Name { get; private set; }
        [ProtoMember(2)]
        public string Password { get; private set; }

        public ClientLobbyJoinMessage()
            : base(ClientMessageType.LOBBY_JOIN, MessageTarget.LOBBBY) { }

        public ClientLobbyJoinMessage(string name, string password)
            : base(ClientMessageType.LOBBY_JOIN, MessageTarget.LOBBBY)
        {
            Name = name;
            Password = password.Trim();
        }
    }
}
