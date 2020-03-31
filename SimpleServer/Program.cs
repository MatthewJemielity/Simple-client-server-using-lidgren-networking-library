using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            Client client1 = new Client();
            Client client2 = new Client();
            Console.ReadKey();
        }
    }
}
