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
        public static void ClientRun()
        {
            Client client = new Client();


            while (true)
            {
                client.Run();
            }
        }

        public static void ServerRun()
        {
            Server server = new Server();

            try
            {

                while (true)
                {
                    server.Receive();
                    server.Switch();
                }
            }
            catch (IOException a)
            {
                Console.WriteLine(a);
            }
        }
    }
}
