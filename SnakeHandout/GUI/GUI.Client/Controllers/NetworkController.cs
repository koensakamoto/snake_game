using CS3500.Networking;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text.Json;
using GUI.Client.Models;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Net.Quic;
using System.Diagnostics;
using System.Net;
using System.IO.Pipes;
using Microsoft.AspNetCore.Components;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace GUI.Client.Controllers
{
    public class NetworkController
    {

        private NetworkConnection serverConnection = new();

        private World world2 = new(0);

        public World world { get; private set; }

        public int thisID { get; private set; } = -1;

        public bool recievedData { get; private set; }


        public void NetworkLoop(string host, int port, string name)
        {

            try
            {
                serverConnection.Connect(host, port); // Connect to server

                if (serverConnection.IsConnected)
                {
                    // Send the name to the server
                    serverConnection.Send(name);

                    // Read the player ID and parse it
                    thisID = int.Parse(serverConnection.ReadLine());

                    // Read the world size and parse it
                    int worldSize = int.Parse(serverConnection.ReadLine());

                    // Initialize the world
                    world = new World(worldSize);

                    recievedData = true;

                    // Start a new thread to update the world
                    new Thread(UpdateWorld).Start();
                }
                else
                {
                    Console.WriteLine("Connection to the server failed.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
        }


        public bool IsConnected
        {
            get
            {
                return serverConnection.IsConnected;
            }
        }


        public void DisconnectFromServer()
        {
            serverConnection?.Disconnect();
        }

        //possibly delete
        public World copyWorld()
        {
            World copyOfWorld = new(0);
            lock (world)
            {
                copyOfWorld = new(world);
            }
            return copyOfWorld;
        }


        private void UpdateWorld()
        {
            while (IsConnected)
            {
                string sentInformation = serverConnection.ReadLine();

                lock (world)
                {
                    if (sentInformation.Contains("snake"))//server sent us a snake
                    {
                        Snake? snake = JsonSerializer.Deserialize<Snake>(sentInformation);

                        if (!world.snakes.TryAdd(snake!.ID, snake))
                        {
                            world.snakes[snake!.ID] = snake;
                        }

                        if (snake.disconnected)
                        {
                            world.snakes.Remove(snake!.ID);
                        }

                    }

                    else if (sentInformation.Contains("wall"))//server sent us a wall
                    {
                        Wall? wall = JsonSerializer.Deserialize<Wall>(sentInformation);


                        if (world.walls.TryAdd(wall!.ID, wall))
                        {
                            world.walls[wall!.ID] = wall;
                        }

                    }


                    else//server sent us a powerup
                    {
                        Powerup? powerup = JsonSerializer.Deserialize<Powerup>(sentInformation);


                        if (!world.powerups.TryAdd(powerup!.ID, powerup))
                        {
                            world.powerups[powerup!.ID] = powerup;
                        }

                        if (powerup.died)
                        {
                            world.powerups.Remove(powerup!.ID);
                        }
                    }
                }
                //Debug.WriteLine(JsonSerializer.Serialize<World>(world, new JsonSerializerOptions
                //{
                //    WriteIndented = true
                //})); //used to write our world to debug
            }
        }


        public void sendGameCommands(string key)
        {
            ControlCommand controlCommand = new ControlCommand();

            key = key.ToLower();

            if (key.Equals("w"))
            {
                controlCommand.moving = "up";
                string jsonContent = JsonSerializer.Serialize(controlCommand);
                //Debug.Write(jsonContent.ToString());
                serverConnection.Send(jsonContent);
            }

            else if (key.Equals("s"))
            {
                controlCommand.moving = "down";
                string jsonContent = JsonSerializer.Serialize(controlCommand);
                // Debug.Write(jsonContent.ToString());
                serverConnection.Send(jsonContent);
            }
            else if (key.Equals("a"))
            {
                controlCommand.moving = "left";
                string jsonContent = JsonSerializer.Serialize(controlCommand);
                //Debug.Write(jsonContent.ToString());
                serverConnection.Send(jsonContent);
            }
            else if (key.Equals("d"))
            {
                {
                    controlCommand.moving = "right";
                    string jsonContent = JsonSerializer.Serialize(controlCommand);
                    // Debug.Write(jsonContent.ToString());
                    serverConnection.Send(jsonContent);
                }
            }

        }
    }
}

