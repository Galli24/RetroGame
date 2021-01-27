using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerLobbyPlayerLeftMessage : ServerMessage
    {
        [ProtoMember(1)]
        public string PlayerName { get; private set; }

        [ProtoMember(2)]
        public string NewHost { get; private set; }

        public ServerLobbyPlayerLeftMessage()
            : base (null, ServerMessageType.LOBBY_PLAYER_LEFT, MessageTarget.LOBBY) { }

        public ServerLobbyPlayerLeftMessage(Socket destination, string playerName, string newHost)
            : base(destination, ServerMessageType.LOBBY_PLAYER_LEFT, MessageTarget.LOBBY)
        {
            PlayerName = playerName;
            NewHost = newHost;
        }
    }
}
