using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tri : MonoBehaviour
{
    [SerializeField] private Transform p0;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;

    // a haldedge mesh 
    private HalfedgeMesh hem;

    // variables to hold Unity mesh, vertices and tris
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;


    // Start is called before the first frame update
    void Start()
    {
        // create a tri with three given point position
        hem = new HalfedgeMesh();
        hem.CreateTriangle(p0.position, p1.position, p2.position);



        // plugin the unity mesh we are going to create
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        // update the haldedge when we change positions of vertices
        hem.UpdateTriangle(p0.position, p1.position, p2.position);

        // create the Unity mesh from the haldedge mesh 
        CreateShape();

        // update the Unity mesh when we change positions of vertices 
        UpdateMesh();
    }

    void CreateShape()
    {
        // unity mesh and halfedge mesh should have same number of vertices
        vertices = new Vector3[hem.vertices.Count];

        // unity tris are represented by its 3 vertices, so 1 tri will need 3 vertices
        triangles = new int[hem.faces.Count * 3];


        vertices[0] = hem.vertices[0].position;
        vertices[1] = hem.vertices[1].position;
        vertices[2] = hem.vertices[2].position;

        // can get all the HEM's vertices by iterating around a face
        triangles[0] = hem.vertices.IndexOf(hem.faces[0].halfedge.startVertex);
        triangles[1] = hem.vertices.IndexOf(hem.faces[0].halfedge.next.startVertex);
        triangles[2] = hem.vertices.IndexOf(hem.faces[0].halfedge.next.next.startVertex);
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // update normals for rendering 
        mesh.RecalculateNormals();
    }
}
