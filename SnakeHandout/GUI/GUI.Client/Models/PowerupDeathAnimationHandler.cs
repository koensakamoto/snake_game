// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models PowerupDeathAnimationHandler</summary>
namespace GUI.Client.Models
{
    /// <summary>
    /// Class used to keep track of powerups that have died
    /// </summary>
    public class PowerupDeathAnimationHandler
    {
        /// <summary>
        /// Mapping of powerup to its respective radius size
        /// </summary>
        private Dictionary<Powerup, double> animationDurationMaster = new();
        /// <summary>
        /// constant that represents max size of powerup during animation
        /// </summary>
        public static readonly double maxRadius = 100;
        /// <summary>
        /// constant that represents increments of radius every frame
        /// </summary>
        public static readonly double step = 0.5;

        /// <summary>
        /// Zero argument constructor for creating model
        /// </summary>
        public PowerupDeathAnimationHandler()
        { }

        /// <summary>
        /// used to copy handlers during copying of the world
        /// </summary>
        /// <param name="old"></param>
        public PowerupDeathAnimationHandler(PowerupDeathAnimationHandler old)
        {
            animationDurationMaster = new Dictionary<Powerup, double>(old.animationDurationMaster);

        }
        /// <summary>
        /// inputs the powerup that died into the mapping, threadding compliant
        /// </summary>
        /// <param name="pow">Powerup that has died</param>
        public void powerUpDied(Powerup pow)
        {

            lock (this)
            {
                animationDurationMaster[pow] = 8;
            }

        }

        /// <summary>
        /// returns the double representing the radius of the Powerup inputted
        /// </summary>
        /// <param name="pow">Powerup that is requested to see radius of</param>
        /// <returns></returns>
        public double getPowerUpRadius(Powerup pow)
        {
            return animationDurationMaster[pow];
            
        }

        /// <summary>
        /// simulates increase in frame by increasing the radius of the powerup
        /// removing those whose animation is complete (i.e. they have reached max radius)
        /// </summary>
        public void frameUp()
        {
            foreach (var pow in animationDurationMaster.Keys)
            {
                if (animationDurationMaster[pow] >= maxRadius)
                {
                    animationDurationMaster.Remove(pow);
                }
                else
                {
                    animationDurationMaster[pow] += step;
                }
            }
        }
        /// <summary>
        /// gets the powerups whose animations are still ongoing (for view use)
        /// </summary>
        /// <returns></returns>
        public Powerup[] getPowerups()
        {
            return animationDurationMaster.Keys.ToArray();
        }

    }
}
