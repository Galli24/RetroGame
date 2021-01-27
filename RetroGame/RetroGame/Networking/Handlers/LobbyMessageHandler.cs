using LibNetworking.Messages.Server;
using RetroGame.Scenes;
using RetroGame.Services;
using System.Diagnostics;

namespace RetroGame.Networking.Handlers
{
    class LobbyMessageHandler
    {
        public static void OnLobbyMessage(ServerMessage message)
        {
            switch (message.ServerMessageType)
            {
                case ServerMessageType.LOBBY_CREATED:
                    Trace.WriteLine("Response LOBBY_CREATED");
                    OnLobbyCreated(message as ServerLobbyCreatedMessage);
                    break;
                case ServerMessageType.LOBBY_JOINED:
                    Trace.WriteLine("Response LOBBY_JOINED");
                    OnLobbyJoined(message as ServerLobbyJoinedMessage);
                    break;
                case ServerMessageType.LOBBY_STARTED:
                    Trace.WriteLine("Response LOBBY_STARTED");
                    break;
                case ServerMessageType.LOBBY_PLAYER_JOINED:
                    Trace.WriteLine("Response LOBBY_PLAYER_JOINED");
                    OnLobbyPlayerJoined(message as ServerLobbyPlayerJoinedMessage);
                    break;
                case ServerMessageType.LOBBY_PLAYER_READY:
                    Trace.WriteLine("Response LOBBY_PLAYER_READY");
                    OnLobbyPlayerReady(message as ServerLobbyPlayerReadyMessage);
                    break;
                case ServerMessageType.LOBBY_PLAYER_LEFT:
                    Trace.WriteLine("Response LOBBY_PLAYER_LEFT");
                    OnLobbyPlayerLeft(message as ServerLobbyPlayerLeftMessage);
                    break;
                default:
                    return;
            }
        }

        private static void OnLobbyCreated(ServerLobbyCreatedMessage message)
        {
            if (!message.HasJoined)
            {
                if (SceneManager.Instance.CurrentScene is LobbyHostScene)
                {
                    var scene = SceneManager.Instance.CurrentScene as LobbyHostScene;
                    RenderService.Instance.DoInRenderThread(() => scene.OnCreateFailed(message.Reason));
                }
            } else
            {
                LobbyManager.Instance.OnLobbyCreated(message);
                RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new LobbyScene()));
            }
        }

        private static void OnLobbyJoined(ServerLobbyJoinedMessage message)
        {
            if (!message.HasJoined)
            {
                if (SceneManager.Instance.CurrentScene is LobbyJoinScene)
                {
                    var scene = SceneManager.Instance.CurrentScene as LobbyJoinScene;
                    RenderService.Instance.DoInRenderThread(() => scene.OnJoinFailed(message.Reason));
                }
            }
            else
            {
                LobbyManager.Instance.OnLobbyJoined(message);
                RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new LobbyScene()));
            }
        }

        private static void OnLobbyPlayerJoined(ServerLobbyPlayerJoinedMessage message)
        {
            Trace.WriteLine("Player joined: " + message.PlayerName);
            LobbyManager.Instance.OnPlayerJoin(message.PlayerName);
        }

        private static void OnLobbyPlayerReady(ServerLobbyPlayerReadyMessage message)
        {
            Trace.WriteLine("Player ready: " + message.PlayerName + " " + message.Ready);
            LobbyManager.Instance.OnPlayerReady(message.PlayerName, message.Ready);
        }

        private static void OnLobbyPlayerLeft(ServerLobbyPlayerLeftMessage message)
        {
            Trace.WriteLine("Player left: " + message.PlayerName);
            LobbyManager.Instance.OnPlayerLeft(message.PlayerName);
        }
    }
}
