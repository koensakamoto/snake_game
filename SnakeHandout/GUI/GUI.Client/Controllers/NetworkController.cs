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
using MySql.Data.MySqlClient;
using System.Reflection.PortableExecutable;

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
        private NetworkConnection gameServerConnection = new();

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
        /// The connection string.
        /// Your uID login name serves as both your database name and your uid
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;" +
      "database=u1466090;" +
      "uid=u1466090;" +
      "password=hey";

        ///<summary>
        ///the integer game id representing the most recent game
        ///</summary>
        public int thisGame { get; private set; }

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

            gameServerConnection.Connect(host, port);//connect to server


            gameServerConnection.Send(name);//send name


            thisID = int.Parse(gameServerConnection.ReadLine());//player id


            int worldSize = int.Parse(gameServerConnection.ReadLine());

            world = new World(worldSize);

            handShake = true;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText += $"insert into Games (StartTime) values(\"{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}\");";
                    command.CommandText += "select last_insert_id();";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            thisGame = reader.GetInt32(0);
                    }
                }
                catch (Exception)
                {
                  //failure to reach database
                }
            }

#pragma warning disable CA1416
            new Thread(() => UpdateWorld()).Start();
#pragma warning restore CA1416


        }

        /// <summary>
        /// Returns whether or not the connecttion is still active
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return gameServerConnection.IsConnected;
            }
        }

        /// <summary>
        /// Disconnects the connects from the server, catching any exceptions generated
        /// by this action
        /// </summary>
        public void DisconnectFromServer()
        {
            try { gameServerConnection?.Disconnect(); }
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

            if (userPrompted)
            {
                errorMessage = "You have disconnected from the server";
            }
            else if (!disconnected)//to prevent multiple assignments to errorMessage
            {
                errorMessage = "There was an error connecting to the server";
            }
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText += $"UPDATE Players SET LeaveTime = \"{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}\" " +
                        $"where LeaveTime is null;";
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    //failure to reach database
                }
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
                    sentInformation = gameServerConnection.ReadLine();


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



                        if (!world.snakes.TryAdd(snake!.ID, snake))//was the snake already in the table
                        {
                            world.snakes[snake!.ID] = snake;
                        }
                        else//this was a newly added snake
                        {
                            //First time seeing this player, add to Player table
                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                try
                                {
                                    // Open a connection
                                    conn.Open();
                                    MySqlCommand command = conn.CreateCommand();

                                    command.CommandText += $"insert into Players (Name, MaxScore, EnterTime, GameID) values(\"{snake.name}\", {snake.maxScore}, " +
                                        $"\"{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}\", {thisGame});";
                                   command.ExecuteNonQuery();
                                }
                                catch (Exception)
                                {
                                   //failure to reach database
                                }

                            }
                        }
                        if (snake.score > snake.maxScore)//update snake's max score
                        {
                            snake.maxScore = snake.score;
                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                try
                                {
                                    // Open a connection
                                    conn.Open();
                                    MySqlCommand command = conn.CreateCommand();

                                    command.CommandText += $"update Players " +
                                        $"set MaxScore = {snake.maxScore} where Name = \"{snake.name}\";";
                                    command.ExecuteNonQuery();
                                }
                                catch (Exception)
                                {
                                   //failure to reach database
                                }

                            }
                        }

                        if (snake.disconnected)
                        {
                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                try
                                {
                                    // Open a connection
                                    conn.Open();
                                    MySqlCommand command = conn.CreateCommand();

                                    command.CommandText += $"update Players" +
                                    $" SET LeaveTime = \"{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}\" WHERE Name = \"{snake.name}\" AND GameID = {thisGame};";
                                    command.ExecuteNonQuery();
                                }
                                catch (Exception)
                                {
                                   
                                }
                            }
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
            ControlCommand controlCommand = new ControlCommand();

            key = key.ToLower();

            if (key.Equals("w") || key.Equals("arrowup"))
            {
                controlCommand.moving = "up";
                string jsonContent = JsonSerializer.Serialize(controlCommand);
                gameServerConnection.Send(jsonContent);

            }

            else if (key.Equals("s") || key.Equals("arrowdown"))
            {
                controlCommand.moving = "down";
                string jsonContent = JsonSerializer.Serialize(controlCommand);

                gameServerConnection.Send(jsonContent);

            }

            else if (key.Equals("a") || key.Equals("arrowleft"))
            {
                controlCommand.moving = "left";
                string jsonContent = JsonSerializer.Serialize(controlCommand);

                gameServerConnection.Send(jsonContent);

            }
            else if (key.Equals("d") || key.Equals("arrowright"))
            {

                controlCommand.moving = "right";
                string jsonContent = JsonSerializer.Serialize(controlCommand);

                gameServerConnection.Send(jsonContent);


            }
        }



    }
}

