using ProtoBuf;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientLobbyCreateMessage : ClientMessage
    {
        [ProtoMember(1)]
        public string Name { get; private set; }
        [ProtoMember(2)]
        public bool HasPassword { get; private set; }
        [ProtoMember(3)]
        public string Password { get; private set; }
        [ProtoMember(4)]
        public ushort Slots { get; private set; }

        public ClientLobbyCreateMessage()
            : base(ClientMessageType.LOBBY_CREATE, MessageTarget.LOBBY) { }

        public ClientLobbyCreateMessage(string name, string password)
            : base(ClientMessageType.LOBBY_CREATE, MessageTarget.LOBBY)
        {
            Name = name.Trim();
            if (!string.IsNullOrEmpty(password.Trim()))
                HasPassword = true;
            Password = password.Trim();
            // This is set to 2 for now
            // In the future this may be ajustable
            Slots = 2;
        }
    }
}
