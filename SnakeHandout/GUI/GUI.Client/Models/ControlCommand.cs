using System.Text.Json.Serialization;

namespace GUI.Client.Models
{
    public class ControlCommand
    {
        [JsonInclude]
        public string moving {  get; set; } 

        public ControlCommand()
        {

        }

    }
}
