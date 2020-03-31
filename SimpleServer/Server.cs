using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleServer
{

    public class Server
    {
        NetServer netServer;
        List<NetPeer> clients;
        public Server()
        {
            var config = new NetPeerConfiguration("hej")
            { Port = 7171 };
            netServer = new NetServer(config);
            netServer.Start();

            if (netServer.Status == NetPeerStatus.Running)
            {
                Console.WriteLine("Server is running on port " + config.Port);
            }
            else
            {
                Console.WriteLine("Server not started...");
            }
            clients = new List<NetPeer>();

            Thread thread1 = new Thread(ReadMessages);
            thread1.Start();
        }

        public void ReadMessages()
        {
            while (true)
            {
                NetIncomingMessage message;
                while ((message = netServer.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("i got smth!");
                            var data = message.ReadString();
                            Console.WriteLine(data);
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            Console.WriteLine(message.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            Console.WriteLine(message.SenderConnection.Status);
                            if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                            {
                                clients.Add(message.SenderConnection.Peer);
                                Console.WriteLine($"{message.SenderConnection.Peer.Configuration.LocalAddress} has connected.");
                            }
                            if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                            {
                                clients.Remove(message.SenderConnection.Peer);
                                Console.WriteLine($"{message.SenderConnection.Peer.Configuration.LocalAddress} has disconnected.");
                            }
                            break;
                        default:
                            Console.WriteLine($"Unhandled message type: {message.MessageType}");
                            break;
                    }
                    netServer.Recycle(message);
                }
                Thread.Sleep(3);
            }
        }
    }
}
