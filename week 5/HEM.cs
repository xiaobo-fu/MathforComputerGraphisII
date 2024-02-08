using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class HEM : MonoBehaviour
{
    // plgu in a Unity object that we want to get mesh from
    [SerializeField] Object gameObject;

    // a haldedge mesh 
    private HalfedgeMesh hem;

    // a input unity mesh we get from the gameObject
    private Mesh inputMesh;

    // variables to hold output Unity mesh
    private Mesh outputMesh;


    // Start is called before the first frame update
    void Start()
    {
        // get the inputMesh from the game object
        inputMesh = gameObject.GetComponent<MeshFilter>().mesh;
        // create a halfdege mesh according to the inputMesh
        hem = new HalfedgeMesh();
        hem.ConvertFromUnityMesh(inputMesh);

        // set the output mesh
        outputMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = outputMesh;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOutputMesh();

        // here we can do some mesh processing
        // hem.MeshProcessing()

        // update hem incase the input changes
        hem.ConvertFromUnityMesh(inputMesh);
    }

    void UpdateOutputMesh()
    {
        outputMesh.Clear();

        outputMesh.vertices = hem.GetVertices();
        outputMesh.triangles = hem.GetTriangles();
        // update normals for rendering 
        outputMesh.RecalculateNormals();
    }
}
