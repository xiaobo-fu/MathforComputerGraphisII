using UnityEngine;

public class RayCastVisualization : MonoBehaviour
{
    private LineRenderer lineRenderer; // line to represent the ray
    private _Ray ray;
    
    void Start()
    {
        // line 
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 100.0f;
    }

    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // set the distance from the camera to the mouse screen position
        mouseScreenPosition.z = -Camera.main.transform.position.z;

        // start position of the ray, from the camera + a bit off set, so you can see the ray 
        Vector3 positionFrom = Camera.main.transform.position + new Vector3(0.0f,-0.5f,1.0f);

        // convert the screen position to world position, as the direction of the ray
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
   
        ray = new _Ray(positionFrom, mouseWorldPosition);
        ray.UpdateRay(positionFrom, mouseWorldPosition);

        // check intersection
        ray.Intersection();

        // visualize the ray
        lineRenderer.SetPosition(0, ray.PositionFrom); // start position
        lineRenderer.SetPosition(1, ray.PositionFrom+ray.Normal*ray.Max); // end position of the ray, length as max
    }
}
