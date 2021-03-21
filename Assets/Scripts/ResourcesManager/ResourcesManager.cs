using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesManager
{
    public static Material GetMaterial(string index)
    {
        return Resources.Load<Material>($"Materials/{index}");
    }

    public static GameObject GetMesh(string index)
    {
        GameObject go = Resources.Load<GameObject>($"Meshes/{index}");
        if(go == null)
        {
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        return go;
    }
}
