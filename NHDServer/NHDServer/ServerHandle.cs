 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NHDServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int clientIdCheck = _packet.ReadInt();
            string username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected success" +
            $"and now player is {_fromClient}");
            if(_fromClient != clientIdCheck)
            {
                Console.WriteLine($"Player \"{username}\" (ID: {_fromClient}) has assumed the wrong client id {clientIdCheck}!");
            }
            // TODO : send player into the game
            Server.clients[_fromClient].SendIntoGame(username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            Vector3 position = _packet.ReadVector3();
            Quaternion rotation = _packet.ReadQuaternion();

            Server.clients[_fromClient].player.SetPosition(position, rotation);
        }

    }
}
