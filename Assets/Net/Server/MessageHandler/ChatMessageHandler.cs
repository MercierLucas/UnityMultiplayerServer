using Unity.Networking.Transport;

namespace Server
{
    public class ChatMessageHandler
    {
        public static void Handle(DataStreamReader reader)
        {
            NetMessage_ChatMessage message = new NetMessage_ChatMessage(reader);
            Logs.Print($"Received message {message.MessageContent}");
        }
    }
}
