using GameServer.Server;
using System.Numerics;

namespace GameServer.Lobbies
{
    class Player
    {
        // Lobby stuff
        public readonly SocketState State;
        public bool IsHost;
        public bool IsReady;

        // Game stuff
        public Vector2 Position;

        public Player(SocketState state, bool isHost)
        {
            // Lobby stuff
            State = state;
            IsHost = isHost;
            IsReady = false;
        }
    }
}
