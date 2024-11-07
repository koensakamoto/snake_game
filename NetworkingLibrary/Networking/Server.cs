// <copyright file="Server.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
//<author>
//Dominik Jamrich and Koen Sakamoto
// </author>
// <version>
// Version 1.1
// </version>
// <date>
// November 2024
// </date>


using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace CS3500.Networking;

/// <summary>
///   Represents a server task that waits for connections on a given
///   port and calls the provided delegate when a connection is made.
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
    public static void StartServer(Action<NetworkConnection> handleConnect, int port)
    {
        TcpListener listener = new(IPAddress.Any, port);

        listener.Start();

        while (true)//infinite loop
        {
            TcpClient client = listener.AcceptTcpClient(); // Listen for connection

            Console.WriteLine("Accepted a connection");

            new Thread(() => handleConnect(new NetworkConnection(client))).Start();  //start a new thread to handle client
        }
    }
}
