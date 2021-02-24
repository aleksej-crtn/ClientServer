using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Enter port number:");
            int port = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter ip address:");
            string ipAddress = Console.ReadLine();
            try
            {
                Console.Write("Enter X: ");
                string messageX = Console.ReadLine();
                Console.Write("Enter Y: ");
                string messageY = Console.ReadLine();

                Thread t2 = new Thread(delegate ()
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipPoint);
                    byte[] data = Encoding.Unicode.GetBytes($"{messageX};{messageY}");
                    socket.Send(data);

                    data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    Console.WriteLine("Server's response: " + builder.ToString());

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                });
                t2.Start();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            Console.Read();
        }
    }
}