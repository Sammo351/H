using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SecurityDoor : NetworkDevice
{
    public bool Locked = false;
    public Vector3 OpenPosition;
    public Vector3 ClosedPosition;

    public Transform DoorTransform;

    public float Value;
    public float MovementSpeed = 5f;

    void Start()
    {
        Command lockCommand = new Command("Lock");
        lockCommand.OnProcessCommand += Lock;
        Command unlockCommand = new Command("Unlock");
        unlockCommand.OnProcessCommand += Unlock;

        DeviceCommands.Add(lockCommand);
        DeviceCommands.Add(unlockCommand);
        
    }

    private void Lock(string[] args)
    {
        Locked = true;
    }

    private void Unlock(string[] args)
    {
        Locked = false;
    }

    public void Update()
    {
        Vector3 Target = Locked ? ClosedPosition : OpenPosition;
        DoorTransform.localPosition = Vector3.Lerp(DoorTransform.localPosition, Target, Time.deltaTime * MovementSpeed);
    }

    public override string GetData()
    {
        return "Access: " + (Locked ? "Locked" : "Open");
    }

}