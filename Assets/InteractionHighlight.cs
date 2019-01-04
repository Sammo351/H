using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHighlight : MonoBehaviour
{

    public Color HighlightColor;
    private Color defaultColor;
    new public Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultColor = renderer.material.GetColor("_BaseColor");
    }

    public void Highlight()
    {
        renderer.material.SetColor("_BaseColor", HighlightColor);
    }

    public void StopHighlighting()
    {
        renderer.material.SetColor("_BaseColor", defaultColor);
    }
}
