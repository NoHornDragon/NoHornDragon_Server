﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace NHDServer
{
    class Client
    {
        public static int dataBuferSize = 4096;
        public int id;
        public TCP tcp;

        public Client(int clientId)
        {
            id = clientId;
            tcp = new TCP(id);
        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int id;
            private Packet receivedData;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int inputId)
            {
                id = inputId;
            }

            public void Connect(TcpClient inputSocket)
            {
                socket = inputSocket;
                socket.ReceiveBufferSize = dataBuferSize;
                socket.SendBufferSize = dataBuferSize;

                stream = socket.GetStream();

                receivedData = new Packet();
                receiveBuffer = new byte[dataBuferSize];

                stream.BeginRead(receiveBuffer, 0, dataBuferSize, ReceiveCallback, null);

                // TODO : send welcome packet
                ServerSend.Welcome(id, "Welcome to NoHornDragon server");
            }


            public void SendData(Packet packet)
            {
                try
                {
                    if(socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Sending data to player {id} via TCP : {ex}");
                }
            }


            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);
                    if(byteLength <= 0)
                    {
                        // TODO : disconnect
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);

                    // handle data
                    receivedData.Reset(HandleData(data));
                    stream.BeginRead(receiveBuffer, 0, dataBuferSize, ReceiveCallback, null);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error receiving TCP data : {ex}");
                    // TODO : disconnect

                }
            }

            private bool HandleData(byte[] data)
            {
                int packetLength = 0;

                receivedData.SetBytes(data);

                if (receivedData.UnreadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
                {
                    byte[] packetBytes = receivedData.ReadBytes(packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet packet = new Packet(packetBytes))
                        {
                            int packetId = packet.ReadInt();
                            Server.packetHandlers[packetId](id, packet);
                        }
                    });

                    packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        packetLength = receivedData.ReadInt();
                        if (packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (packetLength <= 1)
                    return true;
                return false;
            }

        }

    }
}
