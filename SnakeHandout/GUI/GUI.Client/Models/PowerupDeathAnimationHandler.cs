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

        public static readonly double maxRadius = 100;
        public static readonly int step = 0.5;

        /// <summary>
        /// Zero argument constructor for creating model
        /// </summary>
        public PowerupDeathAnimationHandler()
        { }

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

        public Powerup[] getPowerups()
        {
            return animationDurationMaster.Keys.ToArray();
        }

    }
}
