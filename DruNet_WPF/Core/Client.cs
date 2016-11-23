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
        public int locker = 1;
        private static string _Ip = "127.0.0.1";
        private static int _Port = 1995;
        private string path;
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

            for (int i = 0; i < msg.Length + 1; i++)
            {
                if (i < Buffer.Length)
                {
                    message.Add(Buffer[i]);
                }
                else
                {
                    message.Add(254);
                }
            }
            return message;
        }

        public void Send(byte flaga, string msg)
        {
            List<byte> package = PrepareMessage(flaga, msg);

            for (int i = 0; i < package.Count; i++)
            {
                stream.WriteByte(package[i]);
            }
            message.Clear();
        }


        public byte ReceiveFlag()
        {
            return (byte)stream.ReadByte();
        }


        public string ReceiveMsg()
        {
            List<byte> ReceiverMsg = new List<byte>();
            string data = null;
            byte x;

            do
            {
                x = (byte)stream.ReadByte();
                if (x != 254)
                {
                    ReceiverMsg.Add(x);
                }
            } while (x != 254);

            data = Encoding.Default.GetString(ReceiverMsg.ToArray());

            return data;
        }

        public void CreateDirectory(string message)
        {
            if (locker == 1)
            {
                Print("You have no access! Please LogIn");
            }
            else
            {
                Send(2, message);
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
                Send(3, "");
                Print(ReceiveMsg());
            }
        }

        public void CreateFile(string fileName, string content)
        {
            if (locker == 1)
            {
                Print("You have no access! Please LogIn");
            }
            else
            {
                Send(5, fileName);
                Send(5, content);
            }
        }

        //TODO
        public void ReadFile(string message)
        {
            if (locker == 1)
            {
                Print("You have no access! Please LogIn");
            }
            else
            {
                Console.WriteLine("Insert filename");
                Send(4, message);
                Print(ReceiveMsg());
            }
        }

        //TODO
        public void DeleteFile()
        {
            throw new NotImplementedException();
            Send(4, Console.ReadLine());
            if (ReceiveFlag() == 0)
            {
                Print("File delete successful!");
            }
            else
            {
                Print("File delete error!");
            }
        }

        public void GetPath()
        {
            path = ReceiveMsg();
        }
    }
}
