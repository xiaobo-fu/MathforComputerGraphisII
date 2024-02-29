using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [Range(1.0f, 10f)]
    public float radius = 0.1f;
    public Color color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    public Color specular = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    [Range(0.1f, 5.0f)]
    public float glossiness = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(radius, radius, radius);
    }
}
