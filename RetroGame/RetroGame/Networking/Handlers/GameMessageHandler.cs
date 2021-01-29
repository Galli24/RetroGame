using LibNetworking.Messages.Server;
using RetroGame.Scenes;
using RetroGame.Services;
using System.Diagnostics;

namespace RetroGame.Networking.Handlers
{
    class GameMessageHandler
    {
        public static void OnGameMessage(ServerMessage message)
        {
            switch (message.ServerMessageType)
            {
                case ServerMessageType.GAME_PLAYER_UPDATE:
                    //Trace.WriteLine("Response GAME_PLAYER_POSITION");
                    OnGamePlayerUpdate(message as ServerGamePlayerUpdateMessage);
                    break;
                default:
                    return;
            }
        }

        private static void OnGamePlayerUpdate(ServerGamePlayerUpdateMessage message)
        {
            GameManager.Instance.OnPlayerUpdate(message.Player);
        }
    }
}
