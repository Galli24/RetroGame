using System;
using System.Diagnostics;

namespace GameServer.Game
{
    internal sealed class GameClock
    {
        internal GameClock()
        {
            Reset();
        }

        long count;
        const long nsfactor = 10000000L;
        public bool IsRunning { get; private set; }

        internal static long Frequency { get { return Stopwatch.Frequency; } }
        internal static bool IsHighResolution { get { return Stopwatch.IsHighResolution; } }
        internal static long Timestamp { get { return Stopwatch.GetTimestamp(); } }

        internal TimeSpan Elapsed { get; private set; }
        internal TimeSpan Total { get; private set; }

        internal void Start()
        {
            IsRunning = true;
            count = Timestamp;
        }

        internal void Stop()
        {
            IsRunning = false;
        }

        internal void Reset()
        {
            count = Timestamp;
            Elapsed = TimeSpan.Zero;
            Total = TimeSpan.Zero;
            IsRunning = false;
        }

        internal void Restart()
        {
            Reset();
            Start();
        }

        internal void Step()
        {
            if (IsRunning)
            {
                long last = count;
                count = Timestamp;
                long offset = count - last;
                Elapsed = DeltaToTimeSpan(offset);
                Total += Elapsed;
            }
        }

        private static TimeSpan DeltaToTimeSpan(long delta)
        {
            return TimeSpan.FromTicks((delta * nsfactor) / Frequency);
        }
    };
}