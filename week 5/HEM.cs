using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class HEM : MonoBehaviour
{
    [SerializeField] Object gameObject;

    // a haldedge mesh 
    private HalfedgeMesh hem;

    private Mesh inputMesh;

    // variables to hold Unity mesh, vertices and tris
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;


    // Start is called before the first frame update
    void Start()
    {
        inputMesh = gameObject.GetComponent<MeshFilter>().mesh;
        // create a tri with three given point position
        hem = new HalfedgeMesh();
        hem.ConvertFromUnityMesh(inputMesh);

        // plugin the unity mesh we are going to create
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
        hem.ConvertFromUnityMesh(inputMesh);
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = hem.GetVertices();
        mesh.triangles = hem.GetTriangles();
        // update normals for rendering 
        mesh.RecalculateNormals();
    }
}