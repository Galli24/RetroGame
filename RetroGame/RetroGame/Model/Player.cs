using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroGame.Model
{
    public class Player
    {
        public string Name { get; private set; }
        public bool IsHost { get; set; }
        public bool IsReady { get; set; }

        public Player(string name, bool isHost, bool isReady)
        {
            Name = name;
            IsHost = isHost;
            IsReady = isReady;
        }

    }
}
