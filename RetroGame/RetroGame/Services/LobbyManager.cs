using LibNetworking.Messages.Server;
using RetroGame.Model;
using RetroGame.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Dictionary<string, Player> PlayerList { get; private set; } = new Dictionary<string, Player>();

        public bool IsHost
        {
            get => PlayerList[UserManager.Instance.Username].IsHost;
            private set => PlayerList[UserManager.Instance.Username].IsHost = value;
        }

        public bool IsReady
        {
            get => PlayerList[UserManager.Instance.Username].IsReady;
            private set => PlayerList[UserManager.Instance.Username].IsReady = value;
        }

        #endregion

        public void OnLobbyCreated(ServerLobbyCreatedMessage message)
        {
            LobbyJoined(message.Name, message.HasPassword, message.MaxSlots);
            IsHost = true;
        }

        public void OnLobbyJoined(ServerLobbyJoinedMessage message)
        {
            LobbyJoined(message.Name, message.HasPassword, message.MaxSlots, message.PlayerList);
            PlayerList[message.PlayerList[0]].IsHost = true;
        }

        private void LobbyJoined(string name, bool hasPassword, ushort maxSlots, List<string> playerList = null)
        {
            LobbyName = name;
            HasPassword = hasPassword;
            MaxSlots = maxSlots;

            PlayerList.Clear();
            AddPlayer(UserManager.Instance.Username);
            if (playerList != null)
            {
                foreach (var player in playerList.Where(p => p != UserManager.Instance.Username))
                    AddPlayer(player);
            }
        }

        private void AddPlayer(string name) => PlayerList.Add(name, new Player(name, false, false));

        public void LeaveLobby()
        {
            NetworkManager.Instance.LeaveLobby();
            LobbyName = string.Empty;
            HasPassword = false;
            MaxSlots = 0;
            PlayerList.Clear();
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new LobbyMenuScene()));
        }

        public void OnPlayerJoin(string playerName)
        {
            AddPlayer(playerName);
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.ReloadCurrentScene());
        }

        public void OnPlayerReady(string playerName, bool readyState)
        {
            if (PlayerList.ContainsKey(playerName))
                PlayerList[playerName].IsReady = readyState;
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.ReloadCurrentScene());
        }

        public void OnPlayerLeft(string playerName, string newHost)
        {
            PlayerList.Remove(playerName);
            PlayerList[newHost].IsHost = true;
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.ReloadCurrentScene());
        }

        public bool AllPlayersReady() => PlayerList.All(elt => elt.Value.IsReady);

        public void ToggleReady()
        {
            IsReady = !IsReady;
            NetworkManager.Instance.SendReady(IsReady);
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.ReloadCurrentScene());
        }
    }
}
