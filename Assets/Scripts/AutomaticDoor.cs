using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : Door, IPowerReciever
{
    [Header("Power Settings")]
    public bool RequiresPower;
    public bool HasPower = false;

    public PowerGrid powerGrid;


    private void Start()
    {
        powerGrid.ConnectRecieverToGrid(this);
    }

    public void CloseWithDelay(float t)
    {
        Invoke("Close", t);
    }

    public void OnPowerConnectionLost()
    {
        HasPower = false;
        CanMove = false;
    }

    public void OnPowerConnectionEstablished()
    {
        HasPower = true;
        CanMove = true;
    }

    public override bool CanBeUsed()
    {
        if (RequiresPower && !HasPower)
            return false;

        return true;
    }
}

