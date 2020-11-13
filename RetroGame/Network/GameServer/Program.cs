using System.Threading;

namespace GameServer
{
    class Program
    {
        static void Main()
        {
            var globalManager = new GlobalManager();
            globalManager.Start();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
