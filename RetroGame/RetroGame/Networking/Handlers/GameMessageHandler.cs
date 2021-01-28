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
                case ServerMessageType.GAME_PLAYER_POSITION:
                    Trace.WriteLine("Response GAME_PLAYER_POSITION");
                    OnGamePlayerPosition(message as ServerGamePlayerPositionMessage);
                    break;
                default:
                    return;
            }
        }

        private static void OnGamePlayerPosition(ServerGamePlayerPositionMessage message)
        {
            GameManager.Instance.OnPlayerPositionUpdate(message.PlayerName, message.Position);
        }
    }
}
