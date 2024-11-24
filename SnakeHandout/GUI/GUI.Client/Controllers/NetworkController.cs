// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Snake Game Network Controller (C of MVC)</summary>
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
    /// <summary>
    /// This class represents the controller of our application; it converses with the server and 
    /// creates appropriate models for the view to use
    /// </summary>
    public class NetworkController
    {
        /// <summary>
        /// Connection that is used to connect with server
        /// </summary>
        private NetworkConnection serverConnection = new();

        /// <summary>
        /// World used in modeling the world sent by the server
        /// </summary>
        private World world = new(0);

        /// <summary>
        /// The id that matches this player, i.e. the one
        /// playing on this application
        /// </summary>
        public int thisID { get; private set; } = -1;
        /// <summary>
        /// Has the handshake (name and id and worldsize sent) been completed?
        /// true it has been false it has not
        /// </summary>
        public bool handShake { get; private set; }
        /// <summary>
        /// has the connection been disconnected for some reason?
        /// true if it has. Used in view to display error
        /// </summary>
        public bool disconnected { get; private set; } = false;
        /// <summary>
        /// String representing the error message to be displayed upon error
        /// </summary>
        public string errorMessage { get; private set; } = "101:Page Not Found";
       
        /// <summary>
        /// Was there an error with the name the user inputted (i.e. was it longer than 
        /// 16 char?) true if yes.
        /// </summary>
        public bool NameError { get; private set; }

        /// <summary>
        /// Loop representing the active communication between server and client
        /// </summary>
        /// <param name="host">host name/number (basically server address)</param>
        /// <param name="port">port number</param>
        /// <param name="name">name of the player (or desired name, can be error)</param>
        public void NetworkLoop(string host, int port, string name)
        {
            if (name.Length > 16)//name was too long
            {
                errorMessage = "Please re-enter your name, it needs to be less than 16 char.";
                NameError = true;
                return;
            }

            serverConnection.Connect(host, port);//connect to server
               
            

         
               serverConnection.Send(name);//send name
                              

                thisID = int.Parse(serverConnection.ReadLine());//player id
                                                                

                int worldSize = int.Parse(serverConnection.ReadLine());
               
                world = new World(worldSize);

                handShake = true;

                

#pragma warning disable CA1416 
                new Thread(()=> UpdateWorld()).Start();
#pragma warning restore CA1416
            
        }

        /// <summary>
        /// Returns whether or not the connecttion is still active
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return serverConnection.IsConnected;
            }
        }

        /// <summary>
        /// Disconnects the connects from the server, catching any exceptions generated
        /// by this action
        /// </summary>
        public void DisconnectFromServer()
        {
            try { serverConnection?.Disconnect(); }
            catch (Exception)
            {

            }
            NetworkError(true);

    }
        /// <summary>
        /// Used to handle errors upon disconnection
        /// </summary>
        /// <param name="userPrompted">If true, that means that the user
        /// clicked to disconnect, otherwsie, unexpected error: this changes
        /// the text displayed to the user</param>
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
        /// <summary>
        /// Resolves the error and allows the view to clear up the screen 
        /// </summary>
        public void resolveError()
        {
            disconnected = false;
            NameError = false;
        }

        /// <summary>
        /// Copies the world, obeys principles of threading and race-conditions
        /// </summary>
        /// <returns>returns a copy of the world</returns>
        public World copyWorld()
        {
            World copyOfWorld = new(0);
            lock (world)
            {
                copyOfWorld = new(world);
            }
            return copyOfWorld;
        }

        /// <summary>
        /// updates the world from the information sent by the server
        /// </summary>
        private void UpdateWorld()
        {
            while (IsConnected)
            {
                string sentInformation;
                try
                {
                    sentInformation = serverConnection.ReadLine();
                    

                }
                catch (Exception)
                {
                    NetworkError(false);
                    return;
                }

                lock (world)
                {
                    world.animationHandler.frameUp();
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
                            world.animationHandler.powerUpDied(powerup);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends game commands to the server
        /// </summary>
        /// <param name="key">name of the key that was pressed</param>
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
                    
                }

                else if (key.Equals("s") || key.Equals("arrowdown"))
                {
                    controlCommand.moving = "down";
                    string jsonContent = JsonSerializer.Serialize(controlCommand);

                    serverConnection.Send(jsonContent);
                    
                }

                else if (key.Equals("a") || key.Equals("arrowleft"))
                {
                    controlCommand.moving = "left";
                    string jsonContent = JsonSerializer.Serialize(controlCommand);

                    serverConnection.Send(jsonContent);
                    
                }
                else if (key.Equals("d") || key.Equals("arrowright"))
                {
                    
                        controlCommand.moving = "right";
                        string jsonContent = JsonSerializer.Serialize(controlCommand);

                        serverConnection.Send(jsonContent);
                        
                    
                }
            }
   

        
    }
}

