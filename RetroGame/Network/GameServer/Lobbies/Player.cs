using GameServer.Server;

namespace GameServer.Lobbies
{
    class Player
    {
        public readonly SocketState State;
        public readonly bool IsHost;
        public bool IsReady;

        public Player(SocketState state, bool isHost)
        {
            State = state;
            IsHost = isHost;
            IsReady = false;
        }
    }
}
