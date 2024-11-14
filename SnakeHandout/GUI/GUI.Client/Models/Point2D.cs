using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    public class Point2D
    {
        [JsonInclude]
        public int X { get; private set; }
        [JsonInclude]
        public int Y { get; private set; }

        public Point2D()
        {
            X = -1;
            Y = -1;
        }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }   


    }
}
