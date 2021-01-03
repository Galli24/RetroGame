using GameServer.Configuration;
using GameServer.Database;
using GameServer.Handlers;
using GameServer.Lobbies;
using GameServer.Server;
using System;

namespace GameServer
{
    class GlobalManager
    {
        // Singleton
        public static GlobalManager Instance;

        public readonly Config Config;
        public readonly DatabaseClient DBClient;
        public readonly TCPServer Server;
        public readonly ClientMessageHandler ClientMessageHandler;
        public readonly LobbyManager LobbyManager;

        public GlobalManager()
        {
            if (Instance == null)
                Instance = this;
            else
                return;

            Console.WriteLine("Getting configuration...");
            Config = Config.Parse();
            Console.WriteLine("Configuration loaded");

            DBClient = new DatabaseClient(Config.DatabaseURI);

            Server = new TCPServer(Config.IP, Config.Port);

            LobbyManager = new LobbyManager();

            ClientMessageHandler = new ClientMessageHandler();
        }

        public void Start()
        {
            Console.WriteLine("Starting server...");
            Server.Start();
        }
    }
}
