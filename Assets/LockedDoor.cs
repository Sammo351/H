using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door
{
    public string RequiredKey = "1234";

    internal override void Start()
    {
        base.Start();
        Options.Add(new InteractionOption() { Name = "Lockpick Door", OnInteraction = LockPickCallBack, Validate = IsLocked });
        Options.Add(new InteractionOption() { Name = "Use Key", OnInteraction = UseKey, Validate = HasKey });
    }

    private void UseKey()
    {
        Unlock();
    }

    private void LockPickCallBack()
    {
        TryLock();
    }


    internal override bool CanOpen() { return !Locked && !IsOpen(); }
    internal override bool CanClose() { return !Locked && IsOpen(); }

    private bool IsLocked() { return Locked; }
    private bool HasKey()
    {
        bool hasIt = (interactionSystem != null ? interactionSystem.inventorySystem.HasKey(RequiredKey) : false);
        return hasIt && IsLocked();
    }


    public override void OnInteraction(InteractionSystem system, int option)
    {
        base.OnInteraction(system, option);
    }

    public void TryLock()
    {
        LockPicking.Instance.SetupNewLock();
        LockPicking.Instance.ReadyGame(this, interactionSystem.GetComponent<InventorySystem>());
        LockPicking.Instance.OnExit.AddListener(OnExit);
        interactionSystem.LockPlayer();
    }

    void OnExit()
    {
        interactionSystem.UnlockPlayer();
    }


}
