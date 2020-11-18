using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerLobbyPlayerLeftMessage : ServerMessage
    {
        [ProtoMember(1)]
        public string PlayerName { get; private set; }

        public ServerLobbyPlayerLeftMessage()
            : base (null, ServerMessageType.LOBBY_PLAYER_LEFT) { }

        public ServerLobbyPlayerLeftMessage(Socket destination, string playerName)
            : base(destination, ServerMessageType.LOBBY_PLAYER_LEFT)
        {
            PlayerName = playerName;
        }
    }
}
