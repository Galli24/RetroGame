﻿using ProtoBuf;
using System.Net.Sockets;

namespace LibNetworking.Messages.Server
{
    [ProtoContract]
    public sealed class ServerLobbyStartedMessage : ServerMessage
    {
        [ProtoMember(1)]
        public bool HasStarted { get; private set; }
        [ProtoMember(2)]
        public string Reason { get; private set; }
        [ProtoMember(3)]
        public float TickRate { get; private set; }

        public ServerLobbyStartedMessage()
            : base (null, ServerMessageType.LOBBY_STARTED, MessageTarget.LOBBY) { }

        public ServerLobbyStartedMessage(Socket destination, bool hasStarted, float tickRate = 1, string reason = "")
            : base(destination, ServerMessageType.LOBBY_STARTED, MessageTarget.LOBBY)
        {
            HasStarted = hasStarted;
            Reason = reason;
            TickRate = tickRate;
        }
    }
}
