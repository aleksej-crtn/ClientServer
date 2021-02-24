using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PortsListener
{
    class Program
    {
        static void Main(string[] args)
        {
            bool IsPortAvailable(int port, string ip)
            {
                using (var tcp = new TcpClient())
                {
                    var tcpConnect = tcp.BeginConnect(ip, port, null, null);
                    using (tcpConnect.AsyncWaitHandle)
                    {
                        if (tcpConnect.AsyncWaitHandle.WaitOne(2000, false))
                        {
                            try
                            {
                                tcp.EndConnect(tcpConnect);
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            Console.WriteLine("Enter ip address:");
            string ipAddress = Console.ReadLine();

            Console.WriteLine("Enter port range in X-X format:");
            string portRange = Console.ReadLine();
            
            int[] ports = Array.ConvertAll(portRange.Split('-'), int.Parse);

            for (int i = ports[0]; i <= ports[1]; i++)
            {
                if (IsPortAvailable(i, ipAddress))
                {
                    Console.WriteLine($"Port {i} is available for ip {ipAddress}");
                }
                else
                {
                    Console.WriteLine($"Port {i} isn't available for ip {ipAddress}");
                }
            }
            Console.ReadKey();
        }
    }
}
