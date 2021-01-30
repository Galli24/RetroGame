using LibNetworking.Messages.Server;
using RetroGame.Services;

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
                    GameManager.Instance.OnPlayerUpdate((message as ServerGamePlayerUpdateMessage).Player);
                    break;
                case ServerMessageType.GAME_SYNC_SNAPSHOT:
                    GameManager.Instance.OnSyncSnapshot(message as ServerGameSyncSnapshotMessage);
                    break;
                default:
                    return;
            }
        }
    }
}
