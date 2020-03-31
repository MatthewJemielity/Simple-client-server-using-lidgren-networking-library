using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class PacketType
    {
        public enum Packet_To_Client
        {
            ResponseWalk = 1,
        }

        public enum Packet_To_Server
        {
            RequestWalk = 1,
        }
    }
}
