using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    public enum ServerMessageType
    {
        UNDEFINED,
        // Connect
        CONNECTED,
        NOT_AUTHENTICATED,
        // Lobby
        LOBBY_CREATED,
        LOBBY_JOINED,
        LOBBY_STARTED,
        LOBBY_PLAYER_JOINED,
        LOBBY_PLAYER_READY,
        LOBBY_PLAYER_LEFT,
        // Game
        GAME_PLAYER_UPDATE,
        GAME_SYNC_SNAPSHOT
    }

    [ProtoContract(SkipConstructor = true)]
    // Connect
    [ProtoInclude(3, typeof(ServerConnectedMessage))]
    [ProtoInclude(4, typeof(ServerNotAuthenticatedMessage))]
    // Lobby
    [ProtoInclude(5, typeof(ServerLobbyCreatedMessage))]
    [ProtoInclude(6, typeof(ServerLobbyJoinedMessage))]
    [ProtoInclude(7, typeof(ServerLobbyStartedMessage))]
    [ProtoInclude(8, typeof(ServerLobbyPlayerJoinedMessage))]
    [ProtoInclude(9, typeof(ServerLobbyPlayerReadyMessage))]
    [ProtoInclude(10, typeof(ServerLobbyPlayerLeftMessage))]
    // Game
    [ProtoInclude(11, typeof(ServerGamePlayerUpdateMessage))]
    [ProtoInclude(12, typeof(ServerGameSyncSnapshotMessage))]

    public abstract class ServerMessage : Message
    {
        [ProtoIgnore]
        public Socket Destination { get; private set; }

        [ProtoMember(1)]
        public ServerMessageType ServerMessageType { get; private set; }
        [ProtoMember(2)]
        public MessageTarget MessageTarget { get; private set; }

        protected ServerMessage(Socket destination, ServerMessageType serverMessageType, MessageTarget messageTarget)
            : base(MessageType.SERVER)
        {
            Destination = destination;
            ServerMessageType = serverMessageType;
            MessageTarget = messageTarget;
        }
    }
}
