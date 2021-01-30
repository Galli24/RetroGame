using LibNetworking.Models;
using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerGameSyncSnapshotMessage : ServerMessage
    {
        [ProtoMember(1)]
        public Player[] PlayerList { get; private set; }

        public ServerGameSyncSnapshotMessage()
            : base(null, ServerMessageType.GAME_SYNC_SNAPSHOT, MessageTarget.GAME) { }

        public ServerGameSyncSnapshotMessage(Socket destination, Player[] playerList)
            : base(destination, ServerMessageType.GAME_SYNC_SNAPSHOT, MessageTarget.GAME)
        {
            PlayerList = playerList;
        }
    }
}
