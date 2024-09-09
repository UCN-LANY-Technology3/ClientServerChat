using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPServer
{
    class Program
    {

        private static UdpClient listener;
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Start();
        }

        private static void Start()
        {
            listener = new UdpClient(1339);
            Console.WriteLine("Listening...");
            // Waiting for incoming connections...
            StartAccept();
        }

        private static void StartAccept()
        {
            while (true)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 1339);
                byte[] recievedBytes = listener.Receive(ref ep);
                var recievedData = Encoding.ASCII.GetString(recievedBytes, 0, recievedBytes.Length);
                Console.WriteLine(recievedData);
            }
        }
    }
}
