using Unity.Networking.Transport;
using UnityEngine;

namespace Client
{
    public class EntityMessageHandler
    {
        public static void PlayerAppears(DataStreamReader reader)
        {
            Debug.Log("ok");
            NetMessage_Entity entityMessage = new NetMessage_Entity(reader);
            GameObject go = GameObject.Instantiate(
                GameObject.CreatePrimitive(PrimitiveType.Capsule),
                entityMessage.Entity.Position,
                entityMessage.Entity.Rotation
            );
        }

        public static void PlayerJoined(DataStreamReader reader)
        {
            NetMessage_JoinServer joinServer = new NetMessage_JoinServer(reader);
            Debug.Log($"I now have the UID {joinServer.UID}");
        }

    }
}