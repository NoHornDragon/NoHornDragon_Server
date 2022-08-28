 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHDServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected success" +
            $"and now player is {fromClient}");
            if(fromClient != clientIdCheck)
            {
                Console.WriteLine($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client id {clientIdCheck}!");
            }
            // TODO : send player into the game
        }
    }
}
