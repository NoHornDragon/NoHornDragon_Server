using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHDServer
{
    class ServerSend
    {

        private static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for(int i = 0; i < Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }

        private static void SendTCPDataToAll(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                if (i == exceptClient) continue;
                Server.clients[i].tcp.SendData(packet);
            }
        }

        public static void Welcome(int toClient, string msg)
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(msg);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }
    }
}
