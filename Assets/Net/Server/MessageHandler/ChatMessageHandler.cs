using Unity.Networking.Transport;
using Server.Logs;

public class ChatMessageHandler
{
    public static void Handle(DataStreamReader reader)
    {
        NetMessage_ChatMessage message = new NetMessage_ChatMessage(reader);
        Logs.Print($"Received message {message.MessageContent}");
    }
}