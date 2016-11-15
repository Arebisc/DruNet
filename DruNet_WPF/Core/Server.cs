using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DruNet_WPF.Core
{
    class Server
    {
        private static Server instance;
        private static byte[] Buffer;
        private Socket ConnectSocket;
        private Socket WorkSocket;
        private NetworkStream stream;
        private int loopflag;
        private byte flag;
        private List<byte> package;
        private string data;

        public AddOutput ServerOutput;

        private Server()
        {
            this.ConnectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ConnectSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1995));
            this.ConnectSocket.Listen(0);
            WorkSocket = ConnectSocket.Accept();
            stream = new NetworkStream(WorkSocket);
            package = new List<byte>();
        }

        public static Server Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Server();
                }
                return instance;
            }
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
        public void Send(byte message)
        {
            stream.WriteByte(message);
        }

        //TODO
        public void LogIn()
        {
            Receive();
            if (data == "root")
            {
                Send(1);
                Receive();
                if (data == "root")
                {
                    ServerOutput("Client connected to server!");
                    Send(1);
                    loopflag = 2;
                }
                else
                {
                    ServerOutput("Client used to wrong login");
                    Send(0);
                }
            }
            else
            {
                ServerOutput("Client used to wrong password");
                Send(0);
            }
        }
        //TODO
        public void Switch()
        {
            Receive();
            flag = package[0];
            package.Clear();
            switch (flag)
            {
                case 1:
                    CreateFile();
                    break;
                case 2:
                    EditFile();
                    break;
                case 3:
                    DeleteFile();
                    break;
                case 4:
                    ViewTree();
                    break;

            }

        }

        //TODO
        public void CreateFile()
        {
            throw new NotImplementedException();
        }

        public void EditFile()
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

        public void Disconnect()
        {

        }


        //TODO     
        public void Start()
        {
            loopflag = 1;
            do
            {
                LogIn();
            } while (loopflag == 1);
        }

        public void Function()
        {
            do
            {
                Switch();
            } while (loopflag == 2);
        }
    }
}
