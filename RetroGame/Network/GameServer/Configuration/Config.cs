﻿using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GameServer.Configuration
{
    class Config
    {
        public readonly string IP;
        public readonly ushort Port;
        public readonly string AuthServerURI;
        public readonly string DatabaseURI;

        private static readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RetroGame");
        private static readonly string _configFileName = "gameserver.config";
        private static readonly string _configFilePath = Path.Combine(_directoryPath, _configFileName);

        public Config(string ip, ushort port,
            string authServerUri, string databaseUri)
        {
            IP = ip;
            Port = port;
            AuthServerURI = authServerUri;
            DatabaseURI = databaseUri;
        }

        public static Config Parse()
        {
            if (File.Exists(_configFileName))
            {
                return ParseFile(_configFileName);
            }
            else
            {
                if (!Directory.Exists(_directoryPath))
                    Directory.CreateDirectory(_directoryPath);

                CreateFile(_configFilePath, Encoding.UTF8.GetBytes(Properties.Resources.gameserver));
                return ParseFile(_configFilePath);
            }
        }

        private static void CreateFile(string filePath, byte[] fileContent)
        {
            if (!File.Exists(filePath))
            {
                var file = File.Create(filePath);
                file.Write(fileContent, 0, fileContent.Length);
                file.Close();
            }
        }

        private static Config ParseFile(string filePath)
        {
            using var file = File.OpenText(filePath);
            var serializer = new JsonSerializer();
            return serializer.Deserialize(file, typeof(Config)) as Config;
        }
    }
}
