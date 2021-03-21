using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{

    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [Header("Event")]
    [SerializeField] private GameObjectEventChannelSO goEventChannel;

    // Start is called before the first frame update
    private void Start()
    {
        if(goEventChannel != null) goEventChannel.onEventRaised += SetupTarget;
    }

    private void SetupTarget(GameObject go)
    {
        target = go.transform;
    }

    void LateUpdate()
    {
        if(target == null) return;
        transform.position = target.position + offset;
    }
}
