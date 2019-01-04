using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIndicator : MonoBehaviour
{
    public Renderer[] renderers;
    public PowerGrid powerGrid;
    public float Brightness;
    public Light[] lights;
    public float LightBrightness;

    private void Start()
    {
    }

    private void Update()
    {
        foreach(Renderer renderer in renderers)
            renderer.material.SetColor("_EmissiveColor", powerGrid.IsOn ? Color.white * Brightness : Color.black);

        foreach(Light light in lights)
            light.intensity = powerGrid.IsOn ? LightBrightness : 0f;
    }
}
