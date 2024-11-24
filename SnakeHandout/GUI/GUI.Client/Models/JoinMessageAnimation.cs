using System.Security.Cryptography;

namespace GUI.Client.Models
{
    public class JoinMessageAnimation
    {
        public int frameCount { get; private set; }
        private Dictionary<int, int> messageDurationMaster = new();
        public string message { get; private set; } = string.Empty;
        
        public JoinMessageAnimation()
        {
            frameCount = 0;
        }
        public void pushMessage(string newMessage)
        {
            int subFinish = newMessage.Length;
            lock (this) { 
            messageDurationMaster[frameCount + 10000] = subFinish;
            message += newMessage;
            }
        }

        public void incrementFrame()
        {
            frameCount++;
            lock (this)
            {
                if (messageDurationMaster.ContainsKey(frameCount))
                {
                    message = message.Substring(messageDurationMaster[frameCount]);
                }
            }
        }
    }
}
