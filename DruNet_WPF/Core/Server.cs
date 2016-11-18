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
        private byte flag;
        private List<byte> package;
        private string data;
        private static string _Ip = "127.0.0.1";
        private static int _Port = 1995;
        public PrintOutput PrintOutputOnTextBlock;

        public void Print(string message)
        {
            Console.WriteLine(message);
            PrintOutputOnTextBlock?.Invoke(message);
        }

        public static string Ip
        {
            get { return _Ip; }
            set { _Ip = value; }
        }

        public static int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        private Server()
        {
            this.ConnectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ConnectSocket.Bind(new IPEndPoint(IPAddress.Parse(Ip), Port));
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
            Buffer = new byte[package.Count - 1];

            for (int i = 0; i < package.Count - 1; i++)
            {
                Buffer[i] = package[i + 1];
            }

            data = Encoding.Default.GetString(Buffer);
            Print(data);
        }

        //TODO
        public void Send(byte message)
        {
            stream.WriteByte(message);
        }

        //TODO
        public void LogIn()
        {
            Console.WriteLine(data.Length);
            if (data == "root")
            {
                Print("Login accepted");
                Send(1);
                package.Clear();
                Receive();
                if (data == "root")
                {
                    Print("Client connected to server!");
                    Send(1);
                    Function();
                }
                else
                {
                    Print("Client used wrong login");
                    Send(0);
                }
            }
            else
            {
                Print("Client used to wrong password");
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
                    LogIn();
                    break;
                case 2:
                    break;
                case 3:
                    ViewTree();
                    break;
                case 4:
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
            while (true)
            {
                Switch();

            }
        }

        public void Function()
        {
            Print("Funkcje:");
            Switch();

        }
    }
}
