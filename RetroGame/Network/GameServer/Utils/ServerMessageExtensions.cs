using GameServer.Server;
using LibNetworking.Messages.Server;

namespace GameServer.Utils
{
    public static class ServerMessageExtensions
    {
        public static void Send(this ServerMessage serverMessage)
        {
            TCPServer.SendServerMessage(serverMessage.Destination, serverMessage);
        }
    }
}
