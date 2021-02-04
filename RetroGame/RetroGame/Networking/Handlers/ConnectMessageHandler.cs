using LibNetworking.Messages.Server;
using RetroGame.Scenes;
using RetroGame.Services;
using System.Diagnostics;

namespace RetroGame.Networking.Handlers
{
    class ConnectMessageHandler
    {
        public static void OnConnectMessage(ServerMessage message)
        {
            switch (message.ServerMessageType)
            {
                case ServerMessageType.CONNECTED:
                    Trace.WriteLine("Response CONNECTED");
                    OnConnected(message as ServerConnectedMessage);
                    break;
                case ServerMessageType.NOT_AUTHENTICATED:
                    Trace.WriteLine("Response NOT_AUTHENTICATED");
                    OnNotAuthenticated();
                    break;
                default:
                    return;
            }
        }

        private static void OnConnected(ServerConnectedMessage message)
        {
            if (message.Authorized)
            {
                UserManager.Instance.Authorized = true;
                RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new LobbyMenuScene()));
            }
            else
            {
                if (SceneManager.Instance.CurrentScene is LoginScene scene)
                    RenderService.Instance.DoInRenderThread(() => scene.OnLoginFailed(message.Reason));
            }
        }

        private static void OnNotAuthenticated()
        {
            UserManager.Instance.Logout();
            RenderService.Instance.DoInRenderThread(() => SceneManager.Instance.LoadScene(new LoginScene()));
        }
    }
}
