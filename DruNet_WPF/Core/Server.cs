using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TelnetServer;

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
        private byte locker = 1;
        private List<byte> package;
        private string data;
        private static string _Ip = "127.0.0.1";
        private static int _Port = 1995;
        private FileSystemOperator path;
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
            path = new FileSystemOperator();
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


        public void Receive()
        {
            byte x;

            do
            {
                x = (byte)stream.ReadByte();

                if (x != 254)
                {

                    package.Add(x);
                }

            } while (x != 254);
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

        public void SendFlag(byte message)
        {
            stream.WriteByte(message);
        }

        public void SendMsg(string message)
        {
            Buffer = Encoding.Default.GetBytes(message);
            for (int i = 0; i < message.Length; i++)
            {
                stream.WriteByte(Buffer[i]);
            }

            stream.WriteByte(254);
        }


        public void LogIn()
        {
            if (data == "root")
            {
                Print("Login accepted");
                SendFlag(1);
                package.Clear();
                Receive();
                if (data == "root")
                {
                    Print("Client connected to server!");
                    SendFlag(1);
                    package.Clear();
                    locker = 0;
                }
                else
                {
                    package.Clear();
                    Print("Client used wrong password");
                    SendFlag(0);
                }
            }
            else
            {
                package.Clear();
                Print("Client used to wrong login");
                SendFlag(0);

            }
        }
        
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
                    CreateDirectory();
                    break;
                case 3:
                    ViewTree();
                    break;
                case 4:
                    ReadFile();
                    break;
                case 5:
                    CreateFile();
                    break;
                case 6:
                    path.CurrentPath = GetBackPath(path.CurrentPath);
                    break;
                case 7:
                    GetCurrentPath();
                    break;
                case 0:
                    locker = 1;
                    package.Clear();
                    Print("Client has been logged out successfully");
                    break;
            }
        }


        public void CreateDirectory()
        {
            path.CreateDirectory(data);
        }

        public void CreateFile()
        {
            string filename;
            string filedata;

            filename = data;
            Receive();
            filedata = data;

            path.CreateFile(filename, filedata);
        }


        public void ReadFile()
        {
            string text;
            text = path.ReadFile(data);
            SendMsg(text);
        }


        public void ViewTree()
        {
            string tree = null;
            List<string> listdir = path.ListDirectory(path.CurrentPath);

            if (locker == 0)
            {
                for (int i = 0; i < listdir.Count; i++)
                {
                    tree += listdir[i] + "\n";
                }

                if (tree == null)
                    tree = "Pusty katalog";

                SendMsg(tree);
            }
            Print(tree);
        }

        public string GetBackPath(string input)
        {
            string result = String.Empty;
            var splitted = input.Split('\\').ToList();

            splitted.RemoveAt(splitted.Count - 1);

            result = String.Join("\\", splitted.ToArray());

            return result;
        }

        public void GetCurrentPath()
        {
            SendMsg(path.CurrentPath);
        }

        public void Start()
        {
            while (true)
            {
                package.Clear();
                Switch();
            }
        }
    }
}
