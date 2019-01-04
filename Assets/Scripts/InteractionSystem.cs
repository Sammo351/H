using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public FirstPersonCamera Camera;
    public float InteractionDistance = 3f;

    InteractionHighlight lastHighlight;

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, InteractionDistance))
        {
            if (hit.transform.gameObject.GetComponent<InteractionObject>())
                HandleInteraction(hit.transform.gameObject.GetComponent<InteractionObject>());
            else
                if (hit.transform.gameObject.GetComponentInParent<InteractionObject>())
                HandleInteraction(hit.transform.gameObject.GetComponentInParent<InteractionObject>());
            else
                HandleInteraction(null);

        }
        else
            HandleInteraction(null);

    }

    void HandleInteraction(InteractionObject io)
    {
        if (io == null && lastHighlight != null)
        {
            lastHighlight.StopHighlighting();
            lastHighlight = null;
        }

        if (io == null)
            return;

        if (io.GetComponent<InteractionHighlight>())
        {
            var highlight = io.GetComponent<InteractionHighlight>();

            if (lastHighlight != null && lastHighlight != highlight)
                lastHighlight.StopHighlighting();

            lastHighlight = highlight;
            lastHighlight.Highlight();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            io.OnInteraction(this);
        }
    }


}
