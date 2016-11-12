using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DruNet_WPF.Core
{
    class Server
    {
        private byte[] Buffer;
        private Socket ConnectSocket;
        private Socket WorkSocket;
        private NetworkStream stream;
        private byte flag;
        private List<byte> package;
        private string data;

        public AddOutput ClientOutput;

        public Server()
        {
            this.ConnectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ConnectSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1995));
            this.ConnectSocket.Listen(0);
            WorkSocket = ConnectSocket.Accept();
            stream = new NetworkStream(WorkSocket);
           
            package = new List<byte>();
        }
        
        //TODO
        public void Receive()
        {
            for (int i = 0; i < 32; i++)
            {
                byte x = (byte)stream.ReadByte();

                if (x != 0)
                {
                    package.Add(x);
                }
            }

            ConvertReceivedData();
        }
       
        public void ConvertReceivedData()
        {
            Buffer = new byte[32];

            for (int i = 1; i < package.Count; i++)
            {
                Buffer[i - 1] = package[i];
            }

            data = Encoding.Default.GetString(Buffer);
            Console.WriteLine(data);
        }

        //TODO
        public void Send()
        {
            throw new NotImplementedException();
        }
        
        //TODO
        public void LogIn()
        {
            package.Clear();
            Console.WriteLine("Request Login");
            if (data == "root")
            {
                Console.WriteLine("Login accepted");
            }
            else
            {
                Console.WriteLine("Wrong Login. Try again");
            }
            
        }
        
        //TODO
        public void GetFlag()
        {
            throw new NotImplementedException();
        }
        
        //TODO
        public void PackageClear()
        {
            throw new NotImplementedException();
        }
        
        //TODO
        public void Switch()
        {
            flag = package[0];
            switch (flag)
            {
                case 1:
                    LogIn();
                    break;
                case 2:
                    CreateFile();
                    break;
            }

        }
        
        //TODO
        public void CreateFile()
        {
           throw new NotImplementedException();
        }
        
        //TODO
        public void ViewTree()
        {
            throw new NotImplementedException();
        }
        
        //TODO
        public void DeleteFile()
        {
            throw new NotImplementedException();
        }
        
        //TODO
        public void GetAccess()
        {
            throw new NotImplementedException();
        }
    }
}
