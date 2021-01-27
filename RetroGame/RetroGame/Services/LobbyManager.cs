using LibNetworking.Messages.Server;
using RetroGame.Scenes;
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
        public Dictionary<String, bool> PlayerList { get; private set; } = new Dictionary<string, bool>();

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

            PlayerList.Clear();
            PlayerList.Add(UserManager.Instance.Username, false);
            if (playerList != null)
            {
                foreach (var player in playerList.Where(p => p != UserManager.Instance.Username))
                    PlayerList.Add(player, false);
            }
        }

        public void LeaveLobby()
        {
            NetworkManager.Instance.LeaveLobby();
            LobbyName = string.Empty;
            HasPassword = false;
            MaxSlots = 0;
            PlayerList.Clear();
            IsHost = false;

            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new LobbyMenuScene()));
        }

        public void OnPlayerJoin(string playerName)
        {
            PlayerList.Add(playerName, false);
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.ReloadCurrentScene());
        }

        public void OnPlayerReady(string playerName, bool readyState)
        {
            if (PlayerList.ContainsKey(playerName))
                PlayerList[playerName] = readyState;
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.ReloadCurrentScene());
        }

        public void OnPlayerLeft(string playerName)
        {
            PlayerList.Remove(playerName);
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.ReloadCurrentScene());
        }

        public bool AllPlayersReady() => PlayerList.All(elt => elt.Value);
    }
}
