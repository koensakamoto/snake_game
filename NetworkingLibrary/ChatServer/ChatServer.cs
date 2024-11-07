// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
///<author>
///Dominik Jamrich and Koen Sakamoto
/// </author>
/// <version>
/// Version 1.1
/// </version>
/// <date>
/// November 2024
/// </date>

using CS3500.Networking;
using System.IO.Pipes;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CS3500.Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{
    /// <summary>
    /// Dictionary mapping client names with their unique NetworkConnections
    /// </summary>
    private static Dictionary<string, NetworkConnection> clients = new Dictionary<string, NetworkConnection>();

    /// <summary>
    ///   The main program.
    /// </summary>
    /// <param name="args"> ignored. </param>
    /// <returns> A Task. Not really used. </returns>
    private static void Main(string[] args)
    {
        Server.StartServer(HandleConnect, 11_000);
        Console.Read(); // don't stop the program.
    }


    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect(NetworkConnection connection)
    {
        // handle all messages until disconnect.
        try
        {
            string tempName = string.Empty;


            while (true)
            {
                var message = connection.ReadLine();

                if (tempName.Equals(string.Empty))//client doesn't have a name yet
                {
                    lock (clients)
                    {
                        clients.Add(message, connection);
                    }

                    tempName = message;
                }

                else//This isn't the client's first message
                {
                    foreach (var client in clients.Values)
                    {
                        client.Send($"{tempName}: {message}");
                    }


                    connection.Send("thanks!");
                }
            }
        }
        catch (Exception)//there was an exception
        {
            Console.WriteLine("connection closed");
            return;
        }
    }
}