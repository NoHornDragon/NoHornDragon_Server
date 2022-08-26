using System;
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

                receiveBuffer = new byte[dataBuferSize];

                stream.BeginRead(receiveBuffer, 0, dataBuferSize, ReceiveCallback, null);

                // TODO : send welcome packet
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

                    // TODO : handle data
                    stream.BeginRead(receiveBuffer, 0, dataBuferSize, ReceiveCallback, null);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error receiving TCP data : {ex}");
                    // TODO : disconnect

                }
            }
        }

    }
}
