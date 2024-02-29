using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RayTracingMaster : MonoBehaviour
{
    public ComputeShader ComputerShader; // we need a computer shader

    private RenderTexture _target; // the "canvas" we are going to render to

    // some variables to store data eventually passed to the shader
    private Camera _camera; 

    public Light DirectionalLight;

    public GameObject sphere;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // The OnRenderImage function is automatically called by Unity whenever the camera has finished rendering.
    // Here, it will initiate the ray-tracing process and display the result on the screen.
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetShaderParameters();
        Render(destination);
    }

    private void Render(RenderTexture destination)
    {
        InitRenderTexture();

        ComputerShader.SetTexture(0, "Result", _target);

        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);

        // Dispatch creates a grid of work grouds 
        ComputerShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        // Blit the result texture to the screen
        Graphics.Blit(_target, destination);
    }

    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
            {
                _target.Release();
            }
            
            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }

    private void SetShaderParameters()
    {
        // camear
        // Matrix that transforms from camera space to world space
        ComputerShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        // Matrix that transforms from projection space to world space
        ComputerShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);

        // light
        Vector3 l = DirectionalLight.transform.position;
        ComputerShader.SetVector("_DirectionalLight", new Vector4(l.x, l.y, l.z, DirectionalLight.intensity));
        ComputerShader.SetVector("_LightColor", DirectionalLight.color);

        // sphere
        ComputerShader.SetVector("_SpherePosition", sphere.transform.position);
        ComputerShader.SetFloat("_SphereRadius", sphere.GetComponent<Sphere>().radius);
        ComputerShader.SetVector("_SphereColor", sphere.GetComponent<Sphere>().color);
        ComputerShader.SetVector("_SphereSpecular", sphere.GetComponent<Sphere>().specular);
        ComputerShader.SetFloat("_SphereGlossiness", sphere.GetComponent<Sphere>().glossiness);
        // Debug.Log(sphere.transform.localScale);
    }

}