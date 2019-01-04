using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractionObject
{
    public Transform VentObject;

    public Vector3 OpenPosition;
    public Vector3 ClosedPosition;

    public Quaternion OpenRotation;
    public Quaternion ClosedRotation;

    public float PositionSpeed = 5f;
    public float RotationSpeed = 5f;

    public Door[] SubDoors;
    public bool Locked = false;

    internal bool CanMove = true;


    [ContextMenu("Goto Open")]
    void GotoOpen()
    {
        VentObject.localPosition = this.OpenPosition;
        VentObject.localRotation = this.OpenRotation;
    }

    [ContextMenu("Goto Closed")]
    void GotoClosed()
    {
        VentObject.localPosition = this.ClosedPosition;
        VentObject.localRotation = this.ClosedRotation;
    }


    [ContextMenu("Set Open")]
    void SetOpen()
    {
        this.OpenPosition = VentObject.localPosition;
        this.OpenRotation = VentObject.localRotation;
    }

    [ContextMenu("Set Closed")]
    void SetClosed()
    {
        this.ClosedPosition = VentObject.localPosition;
        this.ClosedRotation = VentObject.localRotation;
    }

    bool _open = false;

    public override void OnInteraction(InteractionSystem system)
    {
        base.OnInteraction(system);

        if (Locked)
            return;

        _open = !_open;
        if (!_open)
        {
            foreach (Door d in SubDoors)
                d._open = false;
        }
    }

    public void Open()
    {
        if (!CanBeUsed())
            return;

        _open = true;
    }

    public virtual bool CanBeUsed()
    {
        return true;
    }

    public void Close()
    {
        if (!CanBeUsed())
            return;

        _open = false;
        foreach (Door d in SubDoors)
            d._open = false;
    }

    public void ForceOpen()
    {
        Open();
    }

    public void ForceClosed()
    {
        Close();
    }

    public void Lock() { Locked = true; }
    public void Unlock() { Locked = false; }

    private void Update()
    {
        Vector3 targetPosition = _open ? OpenPosition : ClosedPosition;
        Quaternion targetRotation = _open ? OpenRotation : ClosedRotation;

        if (CanMove)
        {
            VentObject.localPosition = Vector3.Lerp(VentObject.localPosition, targetPosition, PositionSpeed * Time.deltaTime);
            VentObject.localRotation = Quaternion.Lerp(VentObject.localRotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }


}
