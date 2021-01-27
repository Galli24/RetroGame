using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroGame.Configuration
{
    class Config
    {
        public readonly string AuthServerURL;
        public readonly string GameServerIP;
        public readonly ushort GameServerPort;


        private static readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RetroGame");
        private static readonly string _configFileName = "retrogame.config";
        private static readonly string _configFilePath = Path.Combine(_directoryPath, _configFileName);

        public Config(string authServerURL, string gameServerIP, ushort gameServerPort)
        {
            AuthServerURL = authServerURL;
            GameServerIP = gameServerIP;
            GameServerPort = gameServerPort;
        }

        public static Config Parse()
        {
            if (File.Exists(_configFileName))
                return ParseFile(_configFileName);
            else
            {
                if (!Directory.Exists(_directoryPath))
                    Directory.CreateDirectory(_directoryPath);

                CreateFile(_configFilePath, Encoding.UTF8.GetBytes(Properties.Resources.retrogame));
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
