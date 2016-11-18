using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DruNet_WPF.Core
{
    class ApplicationLogicInitializer
    {
        public static void ClientInit(string ip, int port)
        {
            Client.Ip = ip;
            Client.Port = port;
            Client client = Client.Instance;
        }

        public static void ServerInit(string ip, int port)
        {
            Server.Ip = ip;
            Server.Port = port;
            Server server = Server.Instance;
        }
    }
}
