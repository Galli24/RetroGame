using LibNetworking.Models;
using ProtoBuf;
using System.Net.Sockets;
using System.Numerics;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerGamePlayerUpdateMessage : ServerMessage
    {
        [ProtoMember(1)]
        public Player Player { get; private set; }

        public ServerGamePlayerUpdateMessage()
            : base(null, ServerMessageType.GAME_PLAYER_UPDATE, MessageTarget.GAME) { }

        public ServerGamePlayerUpdateMessage(Socket destination, Player player)
            : base(destination, ServerMessageType.GAME_PLAYER_UPDATE, MessageTarget.GAME)
        {
            Player = player;
        }
    }
}
