using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ThirdPersonRenderer : MonoBehaviour
{
    public LayerMask layerMask;

    private void Start()
    {
        SetupRenderers();
    }

    public void SetupRenderers()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.gameObject.AddComponent<HideOnFP>().layerMask = layerMask;
        }
    }


}
