using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : InteractionObject
{
    void Start()
    {
        Options.Add(new InteractionOption() { Name = "Pick up", OnInteraction = PickUpBag, Validate = CanPickup });
    }

    private bool CanPickup()
    {
        return interactionSystem.inventorySystem.Bag == null;
    }

    public override void OnInteraction(InteractionSystem system, int option)
    {
        base.OnInteraction(system, option);


    }

    public void PickUpBag()
    {
        if (interactionSystem.inventorySystem != null)
        {
            interactionSystem.inventorySystem.PickUpBag(this);
        }
    }


}
