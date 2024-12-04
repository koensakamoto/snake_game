// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Server Class (From Chat Assignment)</summary>
using CS3500.Networking;
using System.Net;
using System.Net.Sockets;


namespace WebServer
{
    /// <summary>
    /// Represents server in a client/server connection
    /// </summary>
    public static class Server
    {

        /// <summary>
        ///   Wait on a TcpListener for new connections. Alert the main program
        ///   via a callback (delegate) mechanism.
        /// </summary>
        /// <param name="handleConnect">
        ///   Handler for what the user wants to do when a connection is made.
        ///   This should be run asynchronously via a new thread.
        /// </param>
        /// <param name="port"> The port (e.g., 11000) to listen on. </param>
        public static void StartServer(Action<NetworkConnectionWebServer> handleConnect, int port)
        {
            TcpListener listener = new(IPAddress.Any, port);

            listener.Start();

            while (true)//infinite loop
            {
                TcpClient client = listener.AcceptTcpClient(); // Listen for connection

                Console.WriteLine("Accepted a connection");

                new Thread(() => handleConnect(new NetworkConnectionWebServer(client))).Start();  //start a new thread to handle client
            }
        }
    }
}