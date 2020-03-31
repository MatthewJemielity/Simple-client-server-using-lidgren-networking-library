using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Client
    {
        NetClient netClient;
        public Client()
        {
            var config = new NetPeerConfiguration("hej");
            config.AutoFlushSendQueue = false;
            netClient = new NetClient(config);
            netClient.Start();

            string ip = "localhost";
            int port = 7171;
            netClient.Connect(ip, port);

            Thread thread1 = new Thread(MainLoop);
            thread1.Start();
        }

        public void RequestWalk()
        {
            short x = 100, y = 4, z = 90;
            NetOutgoingMessage message = netClient.CreateMessage();

            message.Write((byte)PacketType.Packet_To_Server.RequestWalk);
            message.Write((short)x);
            message.Write((short)y);
            message.Write((short)z);
            netClient.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            netClient.FlushSendQueue();
        }

        public void MainLoop()
        {
            while (true)
            {
                NetIncomingMessage message;

                while ((message = netClient.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                PacketType.Packet_To_Client type = (PacketType.Packet_To_Client)message.ReadByte();

                                switch (type)
                                {
                                    case PacketType.Packet_To_Client.ResponseWalk:
                                        short newX = message.ReadInt16();
                                        short newY = message.ReadInt16();
                                        short newZ = message.ReadInt16();
                                        //received packet from server with data
                                        break;
                                }
                                break;
                            }
                    }
                }
                Thread.Sleep(3);
            }
        }
    }
}
