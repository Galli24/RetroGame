using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerLobbyPlayerJoinedMessage : ServerMessage
    {
        [ProtoMember(1)]
        public string PlayerName { get; private set; }

        public ServerLobbyPlayerJoinedMessage()
            : base (null, ServerMessageType.LOBBY_PLAYER_JOINED) { }

        public ServerLobbyPlayerJoinedMessage(Socket destination, string playerName)
            : base(destination, ServerMessageType.LOBBY_PLAYER_JOINED)
        {
            PlayerName = playerName;
        }
    }
}
