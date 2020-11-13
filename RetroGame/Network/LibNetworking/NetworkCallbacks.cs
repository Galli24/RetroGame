using LibNetworking.Messages;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System.Net.Sockets;

namespace LibNetworking
{
    public static class NetworkCallbacks
    {
        public delegate void OnMessageDelegate(Message message);
        public static OnMessageDelegate OnMessage { get; set; }

        public delegate void OnClientMessageDelegate(Socket socket, ClientMessage message);
        public static OnClientMessageDelegate OnClientMessage { get; set; }

        public delegate void OnServerMessageDelegate(ServerMessage message);
        public static OnServerMessageDelegate OnServerMessage { get; set; }
    }
}
