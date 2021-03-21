using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;
using Client;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int targetFps;

    [Header("Networking components")]
    [SerializeField] private BaseClient client;

    [Header("Misc")]
    [SerializeField] private GameObject startPanel;

    [Header("Events")]
    [SerializeField] private LogEventChannelSO eventChannel;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;

        startPanel.SetActive(true);
    }

    public void StartNetworkingComponents(bool startServer)
    {
        if(startServer)
        {
            gameObject.AddComponent(typeof(BaseServer));
            eventChannel?.Raise("Server starting, you can now host a game", EventLogType.Warning);
        }
        
        client.Init();

        startPanel.SetActive(false);
        eventChannel?.Raise("Client starting", EventLogType.Warning);
    }

}
