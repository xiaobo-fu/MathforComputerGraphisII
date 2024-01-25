using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    // get access to four data points 
    [SerializeField] private Transform pointP0;
    [SerializeField] private Transform pointP1;
    [SerializeField] private Transform pointP2;
    [SerializeField] private Transform pointP3;

    // variables to hold mesh, vertices and tris
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    // decide the resolution, how many points do we sample
    private int xSize = 20;
    private int zSize = 20;


    // Start is called before the first frame update
    void Start()
    {
        // asign mesh filter that holds vertices and tris
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

    }

    // Update is called once per frame
    void Update()
    {
        // draw and update mesh
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        // need N+1 vertrices to define N segments
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        // get positions for each vertices, loop z, ie, fix z first 
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            // then let x very 
            for (int x = 0; x<= xSize; x++)
            {
                // using lerp succesively to find out position x
                Vector3 r0 = Vector3.Lerp(pointP0.position, pointP2.position, (float)(x)/xSize);
                Vector3 r1 = Vector3.Lerp(pointP1.position, pointP3.position, (float)(x)/xSize);
                Vector3 x0 = Vector3.Lerp(r0, r1, (float)(z)/zSize);

                // assign vertex to the array
                vertices[i] = x0;
                i++;
            }
        };

        // have xSize * zSize * 2 tris, then * 3 vertices
        triangles = new int[xSize * zSize * 6];

        // index for vertices
        int vert = 0;
        // index for vertices represeting tris
        int tris = 0;

        // assign vertices to tris, please check https://www.youtube.com/watch?v=eJEpeUH1EMg, https://www.youtube.com/watch?v=64NblGkAabk&t=1s
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // update normals for rendering 
        mesh.RecalculateNormals();
    }

    // show vertices in scene editor
    void OnDrawGizmos()
    {
        for(int i = 0; i < vertices.Length; i ++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
