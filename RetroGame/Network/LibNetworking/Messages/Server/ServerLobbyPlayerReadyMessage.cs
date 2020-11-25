using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerLobbyPlayerReadyMessage : ServerMessage
    {
        [ProtoMember(1)]
        public string PlayerName { get; private set; }
        [ProtoMember(2)]
        public bool Ready { get; private set; }

        public ServerLobbyPlayerReadyMessage()
            : base(null, ServerMessageType.LOBBY_PLAYER_READY) { }

        public ServerLobbyPlayerReadyMessage(Socket destination, string playerName, bool ready)
            : base(destination, ServerMessageType.LOBBY_PLAYER_READY)
        {
            PlayerName = playerName;
            Ready = ready;
        }
    }
}
