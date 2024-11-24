// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models Powerup</summary>
using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents the powerups sent by the game server
    /// </summary>
    public class Powerup
    {
        /// <summary>
        /// Unique identifier for the powerup
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("power")]
        public int ID { get; set; } 
        /// <summary>
        /// location of the powerup
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("loc")]
        public Point2D location {  get; set; }
        /// <summary>
        /// did the powerup die on this frame? true if yes
        /// </summary>
        [JsonInclude]
        public bool died { get; set; }
        /// <summary>
        /// Constructor for JSON
        /// </summary>
        public Powerup()
        {
          location = new Point2D();
        }
    }
}
