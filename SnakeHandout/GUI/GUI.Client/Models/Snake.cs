// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models Snake</summary>
using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    /// <summary>
    /// Represents snake information sent by the game server
    /// </summary>
    public class Snake
    {
        /// <summary>
        /// Unique identifier for the snake
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("snake")]
        public int ID { get; set; }

        /// <summary>
        /// name of the snake
        /// </summary>
        [JsonInclude]
        public string name { get; set; }

        /// <summary>
        /// List of point2D representing the body of the snake
        /// </summary>
        [JsonInclude]
        public List<Point2D> body { get; set; }
        /// <summary>
        /// Point 2D object representing the direction of the snake
        /// </summary>
        [JsonInclude]
        public Point2D dir { get; set; }
        /// <summary>
        /// Score of the particular snake
        /// </summary>
        [JsonInclude]
        public int score { get; set; }
        /// <summary>
        /// Did the snake die on this frame? true if yes
        /// </summary>
        [JsonInclude]
        public bool died { get; set; }
        /// <summary>
        /// is the snake still alive? true if yes
        /// </summary>
        [JsonInclude]
        public bool alive { get; set; }
        /// <summary>
        /// did the snake disconnect on this frame? true if yes
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("dc")]
        public bool disconnected { get; set; }
        /// <summary>
        /// Did the snake join on this frame? true if yes.
        /// </summary>
        [JsonInclude]
        public bool join { get; set; }
        /// <summary>
        /// Tracks the max score for this snake seen during one 
        /// game.
        /// </summary>
        [JsonIgnore]
        public int maxScore { get; set; }



       /// <summary>
       /// Zero-argument constructor for JSON
       /// </summary>

       public Snake()
        {
            body = new();
            dir = new Point2D();
            name = string.Empty;
        }

        

    }
}
