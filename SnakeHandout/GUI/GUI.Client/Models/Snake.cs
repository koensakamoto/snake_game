using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    public class Snake
    {
        [JsonInclude]
        [JsonPropertyName("snake")]
        public int ID { get; set; }

        [JsonInclude]
        public string name { get; set; }

        [JsonInclude]
        public List<Point2D> body { get; set; }

        [JsonInclude]
        public Point2D dir { get; set; }

        [JsonInclude]
        public int score { get; set; }

        [JsonInclude]
        public bool died { get; set; }

        //public bool dying { get; private set; } = false;

        [JsonInclude]
        public bool alive { get; set; }

        [JsonInclude]
        [JsonPropertyName("dc")]
        public bool disconnected { get; set; }

        [JsonInclude]
        public bool join { get; set; }

        public Snake()
        {

        }

        

    }
}
