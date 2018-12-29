using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HideOnFP : MonoBehaviour
{
    public LayerMask layerMask;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        GameObject fakeRenderer = Instantiate(renderer.gameObject, Vector3.zero, Quaternion.identity);
        renderer.gameObject.layer = LayerMask.NameToLayer("ThirdPerson");

        fakeRenderer.transform.parent = renderer.transform.parent;
        Destroy(fakeRenderer.GetComponent<HideOnFP>());

        fakeRenderer.transform.localPosition = renderer.transform.localPosition;
        fakeRenderer.transform.localRotation = renderer.transform.localRotation;
        fakeRenderer.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;

        fakeRenderer.GetComponent<Renderer>().enabled = true;

    }


}
