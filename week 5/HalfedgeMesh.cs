using System.Collections.Generic;
using UnityEngine;

public class HalfedgeMesh
{
    public class Vertex
    {
        public Vector3 position;
        public Halfedge halfedge;  // One of the half-edges emanating from the vertex

        public Vertex(Vector3 position)
        {
            this.position = position;
        }
    }

    public class Halfedge
    {
        public Vertex startVertex;  // The vertex at the start of the half-edge
        public Halfedge twin;       // The opposite half-edge
        public Halfedge next;       // The next half-edge around the face
        public Halfedge prev;       // The previous half-edge around the face
        public Face face;           // The face this half-edge borders

        public Halfedge(Vertex startVertex)
        {
            this.startVertex = startVertex;
        }
    }

    public class Face
    {
        public Halfedge halfedge;  // One of the half-edges bordering the face

        public Face(Halfedge halfedge)
        {
            this.halfedge = halfedge;
        }
    }

    public List<Vertex> vertices;
    public List<Halfedge> halfedges;
    public List<Face> faces;

    public HalfedgeMesh()
    {
        vertices = new List<Vertex>();
        halfedges = new List<Halfedge>();
        faces = new List<Face>();
    }

    public void ConvertFromUnityMesh(Mesh mesh)
    {
        vertices.Clear();
        halfedges.Clear();
        faces.Clear();

        Dictionary<(int, int), Halfedge> edgeMap = new Dictionary<(int, int), Halfedge>();

        // Step 1: Create vertices
        foreach (Vector3 position in mesh.vertices)
        {
            vertices.Add(new Vertex(position));
        }

        // Step 2: Create faces and half-edges
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int index0 = mesh.triangles[i];
            int index1 = mesh.triangles[i + 1];
            int index2 = mesh.triangles[i + 2];

            Vertex v0 = vertices[index0];
            Vertex v1 = vertices[index1];
            Vertex v2 = vertices[index2];

            Halfedge he0 = new Halfedge(v1);
            Halfedge he1 = new Halfedge(v2);
            Halfedge he2 = new Halfedge(v0);

            he0.next = he1; he1.next = he2; he2.next = he0;
            // Opposites are set up later

            Face face = new Face(he0);
            he0.face = face; he1.face = face; he2.face = face;

            halfedges.AddRange(new[] { he0, he1, he2 });
            faces.Add(face);

            // Add edges to map for opposite linking
            edgeMap[(index0, index1)] = he0;
            edgeMap[(index1, index2)] = he1;
            edgeMap[(index2, index0)] = he2;
        }

        // Step 3: Link opposite half-edges
        foreach (var kvp in edgeMap)
        {
            (int start, int end) = kvp.Key;
            Halfedge currentHE = kvp.Value;
            if (edgeMap.TryGetValue((end, start), out Halfedge oppositeHE))
            {
                currentHE.twin = oppositeHE;
            }
            // Note: Boundary edges may not have an opposite
        }
    }

    public int[] iterateVerticesOfFace(Face face)
    {
        int[] outputVertices = new int[3];
        Halfedge currentHalfedge = face.halfedge;
        for (int i =0; i < 3; i++)
        {
            outputVertices[i] = vertices.IndexOf(currentHalfedge.startVertex);
            currentHalfedge = currentHalfedge.next;          
        }
        return outputVertices;
    }

    public Vector3[] GetVertices() 
    {
        Vector3[] outputVertices = new Vector3[vertices.Count];
        for (int i = 0; i < outputVertices.Length; i++)
        {
            outputVertices[i] = vertices[i].position;

        }
        return outputVertices;

    }

    public int[] GetTriangles() 
    {
        int[] outputTriangles = new int[faces.Count *3];
        for (int i = 0; i < faces.Count; i++)
        {
            int[] tri = iterateVerticesOfFace(faces[i]);
            outputTriangles[i*3] = tri[0];
            outputTriangles[i*3+1] = tri[0+1];
            outputTriangles[i*3+2] = tri[0+2];
        }
        return outputTriangles;
    }
    
}