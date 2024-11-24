// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models Point2D</summary>
using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents the Point2D sent by the server
    /// </summary>
    public class Point2D
    {
        /// <summary>
        /// Represents x value of the point
        /// </summary>
        [JsonInclude]
        public int X { get; private set; }
        /// <summary>
        /// represents y value of the point
        /// </summary>
        [JsonInclude]
        public int Y { get; private set; }
        /// <summary>
        /// Zero argument constructor for Point2D, JSON usage
        /// </summary>
        public Point2D()
        {
            X = -1;
            Y = -1;
        }
    }
}
