using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    public enum ServerMessageType
    {
        UNDEFINED,
        // Connect
        CONNECTED,
        // Lobby
        LOBBY_CREATED,
        LOBBY_JOINED,
        LOBBY_STARTED,
        LOBBY_PLAYER_JOINED,
        LOBBY_PLAYER_READY,
        LOBBY_PLAYER_LEFT
    }

    [ProtoContract(SkipConstructor = true)]
    // Connect
    [ProtoInclude(2, typeof(ServerConnectedMessage))]
    // Lobby
    [ProtoInclude(3, typeof(ServerLobbyCreatedMessage))]
    [ProtoInclude(4, typeof(ServerLobbyJoinedMessage))]
    [ProtoInclude(5, typeof(ServerLobbyStartedMessage))]
    [ProtoInclude(6, typeof(ServerLobbyPlayerJoinedMessage))]
    [ProtoInclude(7, typeof(ServerLobbyPlayerReadyMessage))]
    [ProtoInclude(8, typeof(ServerLobbyPlayerLeftMessage))]
    public abstract class ServerMessage : Message
    {
        [ProtoIgnore]
        public Socket Destination { get; private set; }

        [ProtoMember(1)]
        public ServerMessageType ServerMessageType { get; private set; }

        protected ServerMessage(Socket destination, ServerMessageType serverMessageType)
            : base(MessageType.SERVER)
        {
            Destination = destination;
            ServerMessageType = serverMessageType;
        }
    }
}
