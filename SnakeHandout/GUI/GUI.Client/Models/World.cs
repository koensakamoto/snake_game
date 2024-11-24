// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models World</summary>
using System.Drawing;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents the whole world given to us by snake
    /// </summary>
    public class World
    {
        /// <summary>
        /// Mapping of snakes and their unique ids
        /// </summary>
        public Dictionary<int, Snake> snakes {  get; set; }
        /// <summary>
        /// Mapping of powerups and their unique ids
        /// </summary>
        public Dictionary<int, Powerup> powerups { get; set; }
        /// <summary>
        /// Mapping of walls and their unique ids
        /// </summary>
        public Dictionary<int, Wall> walls { get; set; }
        /// <summary>
        /// size of the world (given by server)
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// Used to map out which powerups have died and need animation
        /// </summary>
        public PowerupDeathAnimationHandler animationHandler { get; set; }


     /// <summary>
     /// constructor used to construct world of given size
     /// </summary>
     /// <param name="size"></param>
        public World(int size)
        {
            snakes = new Dictionary<int, Snake>();
            powerups = new Dictionary<int, Powerup>();
            walls = new Dictionary<int, Wall>();
            this.size = size;
            animationHandler = new();
        }
        /// <summary>
        /// used when copying the world
        /// </summary>
        /// <param name="world">the world to copy</param>
        public World(World world)
        {
            snakes = new(world.snakes);
            powerups = new(world.powerups);
            walls = new(world.walls);
            size = world.size;
            animationHandler = new PowerupDeathAnimationHandler(world.animationHandler);
        }

    }
}
