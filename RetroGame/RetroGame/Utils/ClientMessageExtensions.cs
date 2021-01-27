using LibNetworking.Messages.Client;
using RetroGame.Services;

namespace RetroGame.Utils
{
    public static class ClientMessageExtensions
    {
        public static void Send(this ClientMessage clientMessage)
        {
            NetworkManager.Instance.TCPClient.SendClientMessage(clientMessage);
        }
    }
}
