namespace GUI.Client.Models
{
    public class JoinMessageAnimation
    {

        private Dictionary<string, int> messageDurationMaster = new();

        public JoinMessageAnimation()
        {

        }

        public string MessageToDisplay()
        {
            List<string> removeMessage = new List<string>();
            string message = string.Empty;
            foreach (KeyValuePair<string, int> messageCouplet in messageDurationMaster)
            {
                if (messageCouplet.Value == 100) 
                {
                    removeMessage.Add(messageCouplet.Key);
                }
                else
                {
                    message += messageCouplet.Key + "\n";
                }
            }
            foreach (string s in removeMessage)
            {
                messageDurationMaster.Remove(s);
            }
            return message;
        }

        public void AddSnakes(List<Snake> addedSnakes)
        {
            foreach (Snake snake in addedSnakes) {
                messageDurationMaster.Add(snake.name + " has joined the game!", 0);
            }
        }

        public void DisconnectSnakes(List<Snake> removedSnakes)
        {
            foreach (Snake snake in removedSnakes)
            {
                messageDurationMaster.Add(snake.name + " has left the game!", 0);
            }
        }

        public void incrementFrame()
        {

        }
    }
}
