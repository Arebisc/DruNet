using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DruNet_WPF.Core
{
    class Client
    {
        private static Client instance;
        private byte[] Buffer;
        private Socket ClientSocket;
        private NetworkStream stream;
        private List<byte> message;
        private int locker = 1;
        private static string _Ip = "127.0.0.1";
        private static int _Port = 1995;
        public PrintOutput PrintOutputOnTextBlock;

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

        public void Print(string message)
        {
            Console.WriteLine(message);
            PrintOutputOnTextBlock?.Invoke(message);
        }

        public static Client Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Client();
                }
                return instance;
            }
        }

        private Client()
        {
            try
            {
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);
                ClientSocket.Connect(endPoint);
                stream = new NetworkStream(ClientSocket);
                message = new List<byte>();

                if (ClientSocket.Connected)
                {
                    Print("Połączono z serwerem");
                }
            }
            catch (SocketException a)
            {
                Console.WriteLine(a);
            }
        }

        public List<byte> PrepareMessage(byte flaga, string msg)
        {
            Buffer = Encoding.Default.GetBytes(msg);
            message.Add(flaga);

            for (int i = 0; i < 31; i++)
            {
                if (i < Buffer.Length)
                {
                    message.Add(Buffer[i]);
                }
                else
                {
                    message.Add(0);
                }
            }
            return message;
        }

        public void Send(byte flaga, string msg)
        {
            List<byte> package = PrepareMessage(flaga, msg);

            for (int i = 0; i < 32; i++)
            {
                stream.WriteByte(package[i]);
            }
            message.Clear();
        }

        public byte Receive()
        {
            return (byte)stream.ReadByte();
        }

        public void LogIn()
        {

            Print("Insert Login: ");
            Send(1, Console.ReadLine());
            if (Receive() == 1)
            {
                Print("Insert Password: ");
                Send(1, Console.ReadLine());
                if (Receive() == 1)
                {
                    Print("Connected succesful!");
                    locker = 0;
                }
                else
                {
                    Print("Wrong Password!");
                }
            }
            else
            {
                Print("Wrong Login");
            }

        }

        public void CreateFile()
        {
            Send(2, Console.ReadLine());
            if (Receive() == 0)
            {
                Print("File create successful!");
            }
            else
            {
                Print("File create error!");
            }
        }

        public void ViewTree()
        {
            if (locker == 1)
            {
                Print("You have no access! Please LogIn");
            }
            else
            {
                Send(3, null);
                for (int i = 0; i < Receive(); i++)
                {

                }
            }
        }

        public void DeleteFile()
        {
            Send(4, Console.ReadLine());
            if (Receive() == 0)
            {
                Print("File delete successful!");
            }
            else
            {
                Print("File delete error!");
            }
        }

        public void Run()
        {
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "/login":
                        LogIn();
                        break;
                    case "/ls":
                        ViewTree();
                        break;
                    case "/logout":
                        Print("LogOut!");
                        locker = 1;
                        break;
                }
            }
        }
    }
}
