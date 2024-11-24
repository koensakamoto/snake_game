// <copyright file="NetworkController.cs" 
//<author>Dominik Jamrich and Kevin Sakamoto</author>
//<version>1.0</version>
//<date>November 24, 2024</date>
//<summary>Models Game Controls</summary>
using System.Text.Json.Serialization;

namespace GUI.Client.Models
{ 

    /// <summary>
    /// Provides representation for the control command sent to the server
    /// </summary>
    public class ControlCommand
    {
        /// <summary>
        /// which way is the snake requesting to move
        /// </summary>
        [JsonInclude]
        public string moving {  get; set; } 
        /// <summary>
        /// Basic contstructor (for JSON)
        /// </summary>
        public ControlCommand()
        {
            moving = "none" ;
        }

    }
}
