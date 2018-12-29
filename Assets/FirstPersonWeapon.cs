using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FirstPersonWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.shadowCastingMode = ShadowCastingMode.Off;
        }
    }

}
