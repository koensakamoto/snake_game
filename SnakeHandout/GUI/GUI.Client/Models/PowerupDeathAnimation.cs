// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models PowerupDeathAnimation</summary>
namespace GUI.Client.Models
{
    /// <summary>
    /// Class used to keep track of powerups that have died
    /// </summary>
    public class PowerupDeathAnimation
    {
        /// <summary>
        /// Mapping of powerup to its respective radius size
        /// </summary>
        private Dictionary<Powerup, double> animationDurationMaster = new();
  
        /// <summary>
        /// Zero argument constructor for creating model
        /// </summary>
        public PowerupDeathAnimation()
        { }
        /// <summary>
        /// inputs the powerup that died into the mapping, threadding compliant
        /// </summary>
        /// <param name="pow">Powerup that has died</param>
        public void powerUpDied(Powerup pow)
        {

            lock (this)
            {
                animationDurationMaster[pow] = 7.8;
            }

        }

        /// <summary>
        /// returns the double representing the radius of the Powerup inputted
        /// </summary>
        /// <param name="pow">Powerup that is requested to see radius of</param>
        /// <returns></returns>
        public double getPowerUpRadius(Powerup pow)
        {
            lock (this)
            {
                if (animationDurationMaster[pow] >= 30)
                {
                    animationDurationMaster.Remove(pow);
                    return 0;
                }
                animationDurationMaster[pow] += 0.2;
                return animationDurationMaster[pow];
            }
        }

    }
}
