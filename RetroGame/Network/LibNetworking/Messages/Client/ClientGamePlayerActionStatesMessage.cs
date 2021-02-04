using ProtoBuf;
using LibNetworking.Models;
using System.Collections.Generic;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientGamePlayerActionStatesMessage : ClientMessage
    {
        [ProtoMember(1)]
        public long CurrentClientTick { get; private set; }
        [ProtoMember(2)]
        public Dictionary<Player.Actions, bool> ActionStates { get; private set; }

        public ClientGamePlayerActionStatesMessage()
            : base(ClientMessageType.GAME_PLAYER_ACTION_STATES, MessageTarget.GAME) { }

        public ClientGamePlayerActionStatesMessage(long currentClientTick, Dictionary<Player.Actions, bool> actionStates)
            : base(ClientMessageType.GAME_PLAYER_ACTION_STATES, MessageTarget.GAME)
        {
            CurrentClientTick = currentClientTick;
            ActionStates = actionStates;
        }
    }
}
