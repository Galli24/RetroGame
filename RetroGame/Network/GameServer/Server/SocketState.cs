using System.IO;
using System.Net.Sockets;

namespace GameServer.Server
{
    public class SocketState
    {
        // Socket stuff
        public Socket Socket;
        public bool IsSocketDisposed;
        public byte[] SizeBuffer;
        public byte[] Buffer;
        public MemoryStream Data;

        // Authenticated User stuff
        public bool IsAuthenticated;
        public string UID;
        public string Username;
    }
}
