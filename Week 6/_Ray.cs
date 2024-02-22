using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Ray: MonoBehaviour
{
    public Vector3 PositionFrom;
    public Vector3 PositionTo;
    public float Min;
    public float Max;
    public Vector3 Normal;

    public _Ray(Vector3 positionFrom, Vector3 positionTo, float min=0.0f, float max=999999.0f)
    {
        PositionFrom = positionFrom;
        PositionTo = positionTo;
        Min = min;
        Max = max;
        Normal = positionTo - positionFrom;
        Normal = Normal.normalized;
    }
    
    public void UpdateRay(Vector3 positionFrom, Vector3 positionTo, float min=0.0f, float max=999999.0f)
    {
        PositionFrom = positionFrom;
        PositionTo = positionTo;
        Min = min;
        Max = max;
        Normal = positionTo - positionFrom;
        Normal = Normal.normalized;
    }

    public List<Vector3> Intersection()
    {
        // Find all GameObjects in the scene with MeshRenderer component
        MeshFilter[] allMeshFilters = FindObjectsOfType<MeshFilter>();

        var list = new List<Vector3>();

        foreach (MeshFilter meshFilter in allMeshFilters)
        {
            Debug.Log(meshFilter.gameObject.name);
        }

        return list;
    }
}
