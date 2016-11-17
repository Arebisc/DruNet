﻿using System;
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
            Client client = Client.Instance;
            client.LogIn();
           
        }

        public static void ServerRun()
        {
            Server server = Server.Instance;
            server.Start();
        }
    }
}