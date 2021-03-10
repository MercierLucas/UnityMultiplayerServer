using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Server.Logs;

public class BaseServer : MonoBehaviour
{
    #region CONSTS
    private const int PORT = 5522;
    private const int MAX_CLIENTS = 4;
    
    #endregion

    public NetworkDriver driver;
    protected NativeList<NetworkConnection> connections;

    private NetMessageHandlerManager netMessageHandlerManager;

    #if UNITY_EDITOR
    private void Start(){Init();}
    private void Update(){UpdateServer();}
    private void OnDestroy(){Shutdown();}
    #endif

    public virtual void Init()
    {
        driver =  NetworkDriver.Create();
        NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4;     // Allow anybody to connect to us
        endPoint.Port = PORT;

        netMessageHandlerManager = new NetMessageHandlerManager();

        if(driver.Bind(endPoint) != 0)
        {
            Logs.Error($"Error while binding to {PORT}");
        }
        else
        {
            driver.Listen();
            Logs.Print($"Server running at port {PORT}");
        }

        connections = new NativeList<NetworkConnection>(MAX_CLIENTS, Allocator.Persistent);
    }

    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();         // job is complete so unlock thread
        CleanupConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }

    private void CleanupConnections()
    {
        // Use for clients drop connection without notifying it (atlf4, game or network crash etc.)
        for (int i = 0; i < connections.Length; i++)
        {
            if(!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }

    }

    private void AcceptNewConnections()
    {
        NetworkConnection c;
        while((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
            Logs.Print($"New connection arrived from client");
        }
    }

    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if(cmd == NetworkEvent.Type.Data)
                {
                    netMessageHandlerManager.OnMessageReceived(stream);
                }
                else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    connections[i] = default(NetworkConnection);
                    Logs.Print($"A client disconnected");
                }
            }
        }
    }

    public virtual void Shutdown()
    {
        driver.Dispose();
        connections.Dispose();
    }
}
