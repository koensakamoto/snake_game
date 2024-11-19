using CS3500.Networking;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text.Json;
using GUI.Client.Models;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Net.Quic;
using System.Diagnostics;


namespace GUI.Client.Controllers
{
    public class NetworkController
    {
        private int thisID;

        private int worldSize;

        private World world;

        private NetworkConnection serverConnection = new();

        public bool IsConnected { get {
                return serverConnection.IsConnected;
            } }

        public void NetworkLoop(string name)
        {
            serverConnection.Connect("localhost", 11000);//connect to server

            if (serverConnection.IsConnected)
            {
                Debug.WriteLine("successful connection"); //delete later
            }

            serverConnection.Send(name);//send the name


            thisID = int.Parse(serverConnection.ReadLine());//player id

            Debug.WriteLine(thisID); //delete later

            worldSize = int.Parse(serverConnection.ReadLine());

            world = new World(worldSize);

            //new Thread(() =>
            {
                while (true)
                {

                    string sentInformation = serverConnection.ReadLine();

                    if (sentInformation.Contains("snake"))//server sent us a snake
                    {
                        Snake? snake = JsonSerializer.Deserialize<Snake>(sentInformation);
                        if (!world.snakes.TryAdd(snake!.ID, snake))
                        {
                            world.snakes[snake!.ID] = snake;
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

                    }

                    //Debug.WriteLine(JsonSerializer.Serialize<World>(world, new JsonSerializerOptions
                    //{
                    //    WriteIndented = true
                    //})); //used to write our world to debug
                }
            }//).Start();


        }

        public NetworkController()
        {

        }
    }
}
