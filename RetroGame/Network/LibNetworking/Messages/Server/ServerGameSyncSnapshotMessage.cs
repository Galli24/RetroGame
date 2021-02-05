using LibNetworking.Models;
using ProtoBuf;
using System;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerGameSyncSnapshotMessage : ServerMessage
    {
        [ProtoMember(1)]
        public long CurrentServerTick { get; private set; }
        [ProtoMember(2)]
        public Player[] PlayerList { get; private set; }
        [ProtoMember(3)]
        public Bullet[] BulletList { get; private set; }

        public ServerGameSyncSnapshotMessage()
            : base(null, ServerMessageType.GAME_SYNC_SNAPSHOT, MessageTarget.GAME) 
        {
            PlayerList = Array.Empty<Player>();
            BulletList = Array.Empty<Bullet>();
        }

        public ServerGameSyncSnapshotMessage(Socket destination, long currentServerTick, Player[] playerList, Bullet[] bulletList)
            : base(destination, ServerMessageType.GAME_SYNC_SNAPSHOT, MessageTarget.GAME)
        {
            CurrentServerTick = currentServerTick;
            PlayerList = playerList; 
            BulletList = bulletList;
        }
    }
}
