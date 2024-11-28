using CS3500.Networking;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;

namespace WebServer
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            TcpListener server = new(IPAddress.Any, 10000);
            server.Start();

            NetworkConnection connection = new(server.AcceptTcpClient());
            while (true)
            {
                Console.WriteLine(connection.ReadLine());

            }
        }
    }
}
