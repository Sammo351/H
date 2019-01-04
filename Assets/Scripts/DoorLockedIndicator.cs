using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockedIndicator : MonoBehaviour
{
    public Door door;
    public Color Locked, Unlocked;
    public Renderer[] renderers;
    public Light[] lights;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Renderer renderer in renderers)
            renderer.material.SetColor("_BaseColor", door.Locked ? Locked : Unlocked);

        foreach (Light light in lights)
            light.color = door.Locked ? Locked : Unlocked;
    }
}
