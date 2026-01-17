using UnityEngine;
using System;

public class FogDensityChanger : MonoBehaviour
{
    [Header("Settings")]
    public float targetDensity = 0.08f; // Exponential fog is very thick; 0.05 is a good max
    public float duration = 30f;

    private float timer = 0f;
    private float startDensity = 0.01f;
    public static event Action death;
    void Start()
    {
        // Enable fog and set the mode correctly
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
       
        RenderSettings.fogDensity = startDensity;
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            
            // Smoothly transition the density
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, targetDensity, t);

            if (RenderSettings.fogDensity == targetDensity)
            {death?.Invoke();}
        }
    }
}
