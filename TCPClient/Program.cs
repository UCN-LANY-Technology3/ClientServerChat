using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        private static Socket _sender;
        private static Random _rnd;
        public static void Disconnect(Socket sender)
        {              
            sender.Close();
            sender.Dispose();
            System.Environment.Exit(1);
        }

        static void CurrentDomain_ProcessExit(object s, EventArgs e)
        {            
            Disconnect(_sender);
        }

        public static void SendMessage(string message)
        {

            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).First();
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1337);
            // Create a TCP/IP  socket.
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _sender.Connect(remoteEP);
            Console.WriteLine("Sending message\"{0}\" to: {1}", message, _sender.RemoteEndPoint.ToString());
            byte[] bytes = new byte[1024];
            if (message.Equals("random"))
            {
                
                _rnd.NextBytes(bytes);
                int bytesSent = _sender.Send(bytes);
            }
            else
            {
                byte[] msg = Encoding.UTF8.GetBytes(message);
                int bytesSent = _sender.Send(msg);
            }
            //_sender.Disconnect(true);

            if (message.Equals("quit"))
            {
                Disconnect(_sender);
            }
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            _rnd = new Random();

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            while (true)
            {
                Console.WriteLine("Client App - Type message");
                string message = Console.ReadLine();

                if (message.Length > 0)
                {
                    Console.Clear();
                    SendMessage(message);
                }
            }
        }
    }
}
