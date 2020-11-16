using GameServer.Configuration;
using GameServer.Lobby;
using GameServer.Server;
using System;

namespace GameServer
{
    class GlobalManager
    {
        public readonly Config Config;
        public readonly TCPServer Server;
        public readonly ClientMessageHandler ClientMessageHandler;
        public readonly LobbyManager LobbyManager;

        public GlobalManager()
        {
            Console.WriteLine("Getting configuration...");
            Config = Config.Parse();
            Server = new TCPServer(Config.IP, Config.Port);

            LobbyManager = new LobbyManager();

            ClientMessageHandler = new ClientMessageHandler(Config, LobbyManager);
        }

        public void Start()
        {
            Console.WriteLine("Starting server...");
            Server.Start();
        }
    }
}
