using UnityEngine;
using UnityEngine.Rendering;

public class DisableFogOnCamera : MonoBehaviour
{
    private bool originalFogState;

    void OnEnable() { RenderPipelineManager.beginCameraRendering += OnBeginCamera; }
    void OnDisable() { RenderPipelineManager.beginCameraRendering -= OnBeginCamera; }

    void OnBeginCamera(ScriptableRenderContext context, Camera camera)
    {
        // Only run this for the camera the script is attached to
        if (camera == GetComponent<Camera>())
        {
            RenderSettings.fog = false; // Turn fog OFF for the character
        }
        else
        {
            RenderSettings.fog = true; // Keep fog ON for the world camera
        }
    }
}
