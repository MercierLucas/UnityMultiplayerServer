using UnityEngine;
using System;


[CreateAssetMenu(menuName = "MultiplayerBase (2)/LogEventChannelSO")]
public class LogEventChannelSO : ScriptableObject 
{
    public event Action<string, EventLogType> onEventLogged;

    public void Raise(string eventLog, EventLogType eventType)
    {
        onEventLogged?.Invoke(eventLog, eventType);
    }
}