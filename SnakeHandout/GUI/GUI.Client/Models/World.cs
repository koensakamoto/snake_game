using System.Drawing;

namespace GUI.Client.Models
{
    public class World
    {
        public Dictionary<int, Snake> snakes {  get; set; }

        public Dictionary<int, Powerup> powerups { get; set; }

        public Dictionary<int, Wall> walls { get; set; }

        public int size { get; set; }

     
        public World(int size)
        {
            snakes = new Dictionary<int, Snake>();
            powerups = new Dictionary<int, Powerup>();
            walls = new Dictionary<int, Wall>();
            this.size = size;
        }

        public World(World world)
        {
            snakes = new(world.snakes);
            powerups = new(world.powerups);
            walls = new(world.walls);
            size = world.size;
        }


    }
}
