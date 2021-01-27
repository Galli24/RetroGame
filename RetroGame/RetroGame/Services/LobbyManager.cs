using LibNetworking.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroGame.Services
{
    class LobbyManager
    {
        #region Singleton

        private static LobbyManager _instance;
        public static LobbyManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LobbyManager();
                return _instance;
            }
        }

        #endregion

        #region Members

        public string LobbyName { get; private set; }
        public bool HasPassword { get; private set; }
        public ushort MaxSlots { get; private set; }
        public Dictionary<String, bool> PlayerList { get; private set; }

        public bool IsHost { get; private set; }

        #endregion

        public void OnLobbyCreated(ServerLobbyCreatedMessage message)
        {
            LobbyJoined(message.Name, message.HasPassword, message.MaxSlots);
            IsHost = true;
        }

        public void OnLobbyJoined(ServerLobbyJoinedMessage message)
        {
            LobbyJoined(message.Name, message.HasPassword, message.MaxSlots, message.PlayerList);
        }

        private void LobbyJoined(string name, bool hasPassword, ushort maxSlots, List<string> playerList = null)
        {
            LobbyName = name;
            HasPassword = hasPassword;
            MaxSlots = maxSlots;
            
            PlayerList = new Dictionary<string, bool>();
            PlayerList.Add(UserManager.Instance.Username, false);
            if (playerList != null)
            {
                foreach (var player in playerList)
                    PlayerList.Add(player, false);
            }
        }

        public void LeaveLobby()
        {
            if (PlayerList == null)
                return;

            NetworkManager.Instance.LeaveLobby();
            LobbyName = string.Empty;
            HasPassword = false;
            MaxSlots = 0;
            PlayerList = null;
            IsHost = false;
        }

        public void OnPlayerJoin(string playerName)
        {
            if (PlayerList == null)
                return;

            PlayerList.Add(playerName, false);
        }

        public void OnPlayerReady(string playerName, bool readyState)
        {
            if (PlayerList == null)
                return;

            if (PlayerList.ContainsKey(playerName))
                PlayerList[playerName] = readyState;
        }

        public void OnPlayerLeft(string playerName)
        {
            if (PlayerList == null)
                return;

            PlayerList.Remove(playerName);
        }

        public bool AllPlayersReady() => PlayerList?.All(elt => elt.Value) ?? false;
    }
}
