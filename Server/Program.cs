using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Server
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Enter port number:");
            int port = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter ip address:");
            string ipAddress = Console.ReadLine();

            bool isPortAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isPortAvailable = false;
                    break;
                }
            }

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (isPortAvailable) { 
                listenSocket.Bind(ipPoint);

                listenSocket.Listen(10);

                Console.WriteLine("Server has been started.");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    StringBuilder builder = new StringBuilder();
                    byte[] data = new byte[256];
                    int[] numbers;
                    do
                    {
                        builder.Append(Encoding.Unicode.GetString(data, 0, handler.Receive(data)));
                        numbers = Array.ConvertAll(builder.ToString().Split(';'), int.Parse);
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    Calculator calculator = new Calculator(numbers[0], numbers[1]);

                    handler.Send(Encoding.Unicode.GetBytes($"{calculator.SumCount(calculator.X, calculator.Y)}"));
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            else
            {
                Console.WriteLine("Port is not available");
                Console.ReadKey();
            }
        }
    }
}