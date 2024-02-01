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

    public void CreateTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // Step 1: Create vertices
        Vertex vertex1 = new Vertex(v1);
        Vertex vertex2 = new Vertex(v2);
        Vertex vertex3 = new Vertex(v3);
        vertices.AddRange(new[] { vertex1, vertex2, vertex3 });

        // Step 2: Create half-edges
        Halfedge e0 = new Halfedge(vertex1);
        Halfedge e1 = new Halfedge(vertex2);
        Halfedge e2 = new Halfedge(vertex3);
        Halfedge e3 = new Halfedge(vertex2);
        Halfedge e4 = new Halfedge(vertex1);
        Halfedge e5 = new Halfedge(vertex3);
        halfedges.AddRange(new[] { e0, e1, e2, e3, e4, e5 });

        // Step 3: Create a face
        Face face = new Face(e0);
        faces.Add(face);

        // Step 4: Set up relationships
        // Link half-edges to each other
        e0.next = e1;
        e1.next = e2;
        e2.next = e3;
        e3.next = e5;
        e4.next = e3;
        e5.next = e4;

        e1.prev = e0;
        e2.prev = e1;
        e3.prev = e2;
        e5.prev = e3;
        e3.prev = e4;
        e4.prev = e5;

        //twins
        e0.twin = e3;
        e1.twin = e4;
        e2.twin = e5;
        e3.twin = e0;
        e4.twin = e1;
        e5.twin = e2;

        // Link half-edges to the face
        e0.face = face;
        e1.face = face;
        e2.face = face;

        // Link vertices to one of their half-edges
        vertex1.halfedge = e0;
        vertex2.halfedge = e1;
        vertex3.halfedge = e2;
    }

    public void UpdateTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        vertices[0].position = v1;
        vertices[1].position = v2;
        vertices[2].position = v3;
    }
}