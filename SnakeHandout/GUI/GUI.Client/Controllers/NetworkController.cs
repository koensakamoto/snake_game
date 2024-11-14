using CS3500.Networking;
using System.Net.Sockets;

namespace GUI.Client.Controllers
{
    public class NetworkController
    {
        public void NetworkLoop(string name)
        {
            NetworkConnection serverConnection = new();
            serverConnection.Connect("localhost", 11000);//connect to server

            serverConnection.Send(name);//send the name

            serverConnection.ReadLine();//player id
            Console.WriteLine(serverConnection.ReadLine()); //worldsize


        }

        public NetworkController() { }
    }
}
