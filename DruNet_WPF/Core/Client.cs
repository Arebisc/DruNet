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

        public void LogIn()
        {

            Print("Insert Login: ");
            Send(1, Console.ReadLine());
            if (ReceiveFlag() == 1)
            {
                Print("Insert Password: ");
                Send(1, Console.ReadLine());
                if (ReceiveFlag() == 1)
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

        public void CreateDirectory()
        {


            if (locker == 1)
            {
                Print("You have no access! Please LogIn");
            }
            else
            {
                Print("Inset directory name: ");
                Send(2, Console.ReadLine());
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

        public void CreateFile()
        {
            if (locker == 1)
            {
                Print("You have no access! Please LogIn");
            }
            else
            {
                Print("Insert filename");
                Send(5, Console.ReadLine());
                Print("Insert filedata");
                Send(5, Console.ReadLine());
            }
        }

        public void ReadFile()
        {
            if (locker == 1)
            {
                Print("You have no access! Please LogIn");
            }
            else
            {
                Console.WriteLine("Insert filename");
                Send(4, Console.ReadLine());
                Print(ReceiveMsg());
            }
        }

        public void DeleteFile()
        {
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

        public void Run()
        {
            Print("/login - logowanie");
            Print("/logout - wylogowanie");
            Print("/ls - wyswietla liste katalogow");
            Print("/cd - tworzy nowy katalog");
            Print("/cf - tworzy nowy plik");
            Print("/rf - wyswietla zawartowsc pliku");

            while (true)
            {



                if (stream.DataAvailable)
                {
                    GetPath();
                    Print(path + " ");
                }
                else
                {
                    Print(path + " ");
                }

                switch (Console.ReadLine())
                {
                    case "/login":
                        LogIn();
                        break;
                    case "/ls":
                        ViewTree();
                        break;
                    case "/logout":
                        locker = 1;
                        Send(0, " ");
                        break;
                    case "/cd":
                        CreateDirectory();
                        break;
                    case "/rf":
                        ReadFile();
                        break;
                    case "/cf":
                        CreateFile();
                        break;
                    case "/help":
                        Print("/login - logowanie");
                        Print("/logout - wylogowanie");
                        Print("/ls - wyswietla liste katalogow");
                        Print("/cd - tworzy nowy katalog");
                        Print("/cf - tworzy nowy plik");
                        Print("/rf - wyswietla zawartowsc pliku");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
