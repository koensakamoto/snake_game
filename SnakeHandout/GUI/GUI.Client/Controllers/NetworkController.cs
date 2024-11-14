using CS3500.Networking;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text.Json;
using GUI.Client.Models;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Net.Quic;


namespace GUI.Client.Controllers
{
    public class NetworkController
    {
        private int thisID;

        private int worldSize;

        private World world;

        private NetworkConnection serverConnection;

        public bool IsConnected{ get; private set; }

        public void NetworkLoop(string name)
        {
            serverConnection = new();
            serverConnection.Connect("localhost", 11000);//connect to server

            serverConnection.Send(name);//send the name


            thisID = int.Parse(serverConnection.ReadLine());//player id

            worldSize = int.Parse(serverConnection.ReadLine());

            world = new World(worldSize);

            new Thread(() =>
            {
                while (true)
                {

                    string sentInformation = serverConnection.ReadLine();

                    if (sentInformation.Contains("snake"))//server sent us a snake
                    {
                        Snake? snake = JsonSerializer.Deserialize<Snake>(sentInformation);
                        if (!world.snakes.ContainsKey(snake!.ID))
                        {
                            world.snakes.Add(snake!.ID, snake);
                        }
                        else
                        {
                            world.snakes[snake!.ID] = snake;
                        }

                    }
                    else if (sentInformation.Contains("wall"))//server sent us a wall
                    {
                        Wall? wall = JsonSerializer.Deserialize<Wall>(sentInformation);
                        if (!world.walls.ContainsKey(wall!.ID))
                        {
                            world.walls.Add(wall!.ID, wall); ;
                        }
                        else
                        {
                            world.walls[wall!.ID] = wall;
                        }


                    }
                    else//server sent us a powerup
                    {
                        Powerup? powerup = JsonSerializer.Deserialize<Powerup>(sentInformation);
                        if (!world.powerups.ContainsKey(powerup!.ID))
                        {
                            world.powerups.Add(powerup!.ID, powerup);
                        }
                        else
                        {
                            world.powerups[powerup!.ID] = powerup;
                        }

                    }


                }
            }).Start();


        }

        public NetworkController()
        {

        }
    }
}
