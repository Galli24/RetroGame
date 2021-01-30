using ProtoBuf;
using LibNetworking.Models;

namespace LibNetworking.Messages.Client
{
    [ProtoContract]
    public sealed class ClientGamePlayerKeyStateMessage : ClientMessage
    {
        [ProtoMember(1)]
        public Player.Actions KeyAction { get; private set; }
        [ProtoMember(2)]
        public bool State { get; private set; }

        public ClientGamePlayerKeyStateMessage()
            : base(ClientMessageType.GAME_PLAYER_KEY_STATE, MessageTarget.GAME) { }

        public ClientGamePlayerKeyStateMessage(Player.Actions keyAction, bool state)
            : base(ClientMessageType.GAME_PLAYER_KEY_STATE, MessageTarget.GAME)
        {
            KeyAction = keyAction;
            State = state;
        }
    }
}
