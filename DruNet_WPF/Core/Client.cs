using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DruNet_WPF.Core
{
    class Client
    {
        private byte[] Buffer;
        private Socket ClientSocket;
        private NetworkStream stream;
        private List<byte> message;


        public Client()
        {
            try
            {
                this.ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1995);
                ClientSocket.Connect(endPoint);
                stream = new NetworkStream(ClientSocket);
                message = new List<byte>();
                if (ClientSocket.Connected)
                {
                    Console.WriteLine("Polaczono z serwerem");
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

        public void Receive()
        {

        }

        public void LogIn()
        {
            string login;
            Console.WriteLine("Insert LogIn:");
            login = Console.ReadLine();
            Send(1, login);

        }

        public void CreateFile()
        {
            string filename;
            Console.WriteLine("Insert File Name:");
            filename = Console.ReadLine();
            Send(2, filename);
        }

        public void ViewTree()
        {

        }

        public void DeleteFile()
        {

        }

        public void Run()
        {
            int a;
            while (true)
            {

                Console.WriteLine("1. Login \n 2. Create File");
                a = Convert.ToInt32(Console.ReadLine());

                switch (a)
                {
                    case 1:
                        LogIn();
                        break;
                    case 2:
                        CreateFile();
                        break;
                }
            }
        }
    }
}
