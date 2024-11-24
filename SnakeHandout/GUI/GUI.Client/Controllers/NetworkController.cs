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

        private World world = new(0);

        public int thisID { get; private set; } = -1;

        public bool handShake { get; private set; }

        public bool disconnected { get; private set; } = false;

        public string errorMessage { get; private set; } = "101:Page Not Found";

        private bool sentOnceFrame { get; set; }

        public bool NameError { get; private set; }


        public void NetworkLoop(string host, int port, string name)
        {
            if (name.Length > 16){
                errorMessage = "Please re-enter your name, it needs to be less than 16 char.";
                NameError = true;
                return;
            }

            serverConnection.Connect(host, port);//connect to server
               
            

         
               serverConnection.Send(name);
                              

                thisID = int.Parse(serverConnection.ReadLine());//player id
                                                                

                int worldSize = int.Parse(serverConnection.ReadLine());
               
                world = new World(worldSize);

                handShake = true;

                sentOnceFrame = false;

#pragma warning disable CA1416 
                new Thread(()=> UpdateWorld()).Start();
#pragma warning restore CA1416
            
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
            try { serverConnection?.Disconnect(); }
            catch (Exception)
            {

            }
            NetworkError(true);

    }

        public void NetworkError(bool userPrompted)
        {
            
            if (userPrompted) {
                errorMessage = "You have disconnected from the server";
            }
            else if(!disconnected)//to prevent multiple assignments to errorMessage
            {
                errorMessage = "There was an error connecting to the server";
            }
            disconnected = true;

        }

        public void resolveError()
        {
            disconnected = false;
            NameError = false;
        }

        
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
                string sentInformation;
                try
                {
                    sentInformation = serverConnection.ReadLine();
                    sentOnceFrame = false;

                }
                catch (Exception)
                {
                    NetworkError(false);
                    return;
                }

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
            }
        }


        public void sendGameCommands(string key)
        {
            Debug.WriteLine(key);
            ControlCommand controlCommand = new ControlCommand();

            key = key.ToLower();
            
                if (key.Equals("w") || key.Equals("arrowup"))
                {
                    controlCommand.moving = "up";
                    string jsonContent = JsonSerializer.Serialize(controlCommand);
                    serverConnection.Send(jsonContent);
                    sentOnceFrame = true;
                }

                else if (key.Equals("s") || key.Equals("arrpwdown"))
                {
                    controlCommand.moving = "down";
                    string jsonContent = JsonSerializer.Serialize(controlCommand);

                    serverConnection.Send(jsonContent);
                    sentOnceFrame = true;
                }

                else if (key.Equals("a") || key.Equals("arrowleft"))
                {
                    controlCommand.moving = "left";
                    string jsonContent = JsonSerializer.Serialize(controlCommand);

                    serverConnection.Send(jsonContent);
                    sentOnceFrame = true;
                }
                else if (key.Equals("d") || key.Equals("arrowright"))
                {
                    {
                        controlCommand.moving = "right";
                        string jsonContent = JsonSerializer.Serialize(controlCommand);

                        serverConnection.Send(jsonContent);
                        sentOnceFrame = true;
                    }
                }
            }
   

        
    }
}

