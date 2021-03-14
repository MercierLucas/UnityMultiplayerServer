using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu( menuName = "MultiplayerServerClient/ResourceIndex")]
public class ResourceIndex : ScriptableObject
{
    [Header("Visuals")]
    public List<string> meshes; 
    public List<string> material;



    public Material GetMaterial(byte index)
    {
        if(material.Count > index)
        {
            return Resources.Load<Material>($"Material/{material[index]}");
        }
        return null;
    }

    public GameObject GetMesh(byte index)
    {
        if(meshes.Count > index)
        {
            return Resources.Load<GameObject>($"Mesh/{meshes[index]}");
        }
        return null;
    }
}
