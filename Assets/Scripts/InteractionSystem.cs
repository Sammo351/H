using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public FirstPersonCamera Camera;
    public float InteractionDistance = 3f;
    

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, InteractionDistance))
        {
            if (hit.transform.gameObject.GetComponent<InteractionObject>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    var iObj = hit.transform.gameObject.GetComponent<InteractionObject>();
                    iObj.OnInteraction(this);
                }
            }
            else
            {
                if (hit.transform.gameObject.GetComponentInParent<InteractionObject>())
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        var iObj = hit.transform.gameObject.GetComponentInParent<InteractionObject>();
                        iObj.OnInteraction(this);
                    }
                }
            }
        }

    }
}
