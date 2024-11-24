// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models Wall</summary>
using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents the walls of the game sent by server
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// unique identifier of the wall
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("wall")]
        public int ID { get; set; }
        /// <summary>
        /// one endpoint of the wall
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("p1")]
        public Point2D point1 { get; set; }
        /// <summary>
        /// other endpoint of the wall
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("p2")]
        public Point2D point2 { get; set; }
        /// <summary>
        /// Zero-argument constructor for JSON
        /// </summary>
        public Wall()
        {
            point1 = new Point2D();
            point2 = new Point2D();
        }
    }
}
