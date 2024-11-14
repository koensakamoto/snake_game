using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    public class Wall
    {
        [JsonInclude]
        [JsonPropertyName("wall")]
        public int ID { get; set; }

        [JsonInclude]
        [JsonPropertyName("p1")]
        public Point2D point1 { get; set; }

        [JsonInclude]
        [JsonPropertyName("p2")]
        public Point2D point2 { get; set; }

        public Wall()
        {

        }
    }
}
