using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    public FirstPersonCamera Camera;
    public FirstPersonController characterController;
    public float InteractionDistance = 3f;

    InteractionHighlight lastHighlight;
    public InventorySystem inventorySystem;
    public LayerMask InteractionLayer;

    public Text UI_Header;
    public GameObject UI_Template;
    public List<GameObject> UI_Options = new List<GameObject>();
    public RectTransform UI_ListParent;

    void Start()
    {
        Physics.interCollisionSettingsToggle = false;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, InteractionDistance, InteractionLayer))
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

    internal void LockPlayer()
    {
        Camera.enabled = false;
        characterController.enabled = false;
    }

    internal void UnlockPlayer()
    {
        Camera.enabled = true;
        characterController.enabled = true;
    }

    int optionIndex = 0;
    InteractionObject currentInteractionObject;
    float scrollIndex = 0f;

    public void ForceRedrawOfOptions()
    {
        currentInteractionObject = null;
    }

    void HandleInteraction(InteractionObject io)
    {

        if (io != currentInteractionObject || (io != null && io.IsDirty))
        {
            if (io == null)
                currentInteractionObject.interactionSystem = null;
            else
                io.interactionSystem = this;

            PopulateOptions(io);
            scrollIndex = 0;

            if (io != null && io.IsDirty)
                io.ClearDirty();


        }

        currentInteractionObject = io;

        if (io == null && lastHighlight != null)
        {
            lastHighlight.StopHighlighting();
            lastHighlight = null;
        }



        if (io == null)
            return;


        List<InteractionOption> ActiveOptions = io.Options.Where(a => a.Validate == null || a.Validate.Invoke()).ToList();

        if (io != null && ActiveOptions.Count > 0)
        {
            scrollIndex = Mathf.Clamp(scrollIndex, 0f, ActiveOptions.Count - 1);
            scrollIndex += -Input.mouseScrollDelta.y;
            optionIndex = Mathf.RoundToInt(scrollIndex);
        }



        if (UI_Options.Count >= optionIndex)
            UI_Options[optionIndex].GetComponent<Selectable>().Select();

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
            io.OnInteraction(this, optionIndex);
        }
    }


    void PopulateOptions(InteractionObject interactionObject)
    {
        UI_Options.ForEach(a => Destroy(a.gameObject));
        UI_Options.Clear();
        UI_Header.text = "";
        if (interactionObject != null)
        {
            UI_Header.text = interactionObject.Name;
            List<InteractionOption> ActiveOptions = interactionObject.Options.Where(a => a.Validate == null || a.Validate.Invoke()).ToList();
            foreach (InteractionOption option in ActiveOptions)
            {
                GameObject obj = Instantiate(UI_Template) as GameObject;
                obj.GetComponentInChildren<Text>().text = option.Name;
                obj.transform.SetParent(UI_ListParent);
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
                UI_Options.Add(obj);
            }
        }
    }

}
