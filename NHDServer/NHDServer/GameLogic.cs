using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHDServer
{
    class GameLogic
    {
        public static void Update()
        {
            foreach(Client _client in Server.clients.Values)
            {
                if (_client.player == null) continue;

                _client.player.Update();
            }

            ThreadManager.UpdateMain();
        }
    }
}
