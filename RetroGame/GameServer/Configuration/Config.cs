using System;
using System.IO;
using Newtonsoft.Json;

namespace GameServer.Configuration
{
    class Config
    {
        public string IP { get; private set; }
        public ushort Port { get; private set; }

        private static readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RetroGame");
        private static readonly string _configFileName = "config.json";
        private static readonly string _configFilePath = Path.Combine(_directoryPath, _configFileName);

        public Config(string ip, ushort port)
        {
            IP = ip;
            Port = port;
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

                CreateFile(_configFilePath, Properties.Resources.config);
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
