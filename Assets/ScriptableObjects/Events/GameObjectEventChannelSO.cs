using UnityEngine;
using System;

[CreateAssetMenu(menuName="MultiplayerBase (2)/GameObjectEventChannelSO")]
public class GameObjectEventChannelSO : ScriptableObject 
{
    public event Action<GameObject> onEventRaised;

    public void Raise(GameObject go)
    {
        onEventRaised?.Invoke(go);
    }
}