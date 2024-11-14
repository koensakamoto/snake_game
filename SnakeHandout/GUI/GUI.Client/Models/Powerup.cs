using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    public class Powerup
    {

        [JsonInclude]
        [JsonPropertyName("power")]
        public int ID { get; set; } 

        [JsonInclude]
        [JsonPropertyName("loc")]
        public Point2D location {  get; set; }
        
        [JsonInclude]
        public bool died { get; set; }

        public Powerup()
        {

        }
    }
}
