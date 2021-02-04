using LibNetworking.Messages.Server;
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
                case ServerMessageType.GAME_SYNC_CLOCK:
                    Trace.WriteLine($"Response GAME_SYNC_CLOCK: {(message as ServerGameSyncClockMessage).RequestedClock}");
                    GameManager.Instance.OnSyncClock((message as ServerGameSyncClockMessage).RequestedClock);
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
