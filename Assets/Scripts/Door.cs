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
        _open = !_open;
    }

    private void Update()
    {
        Vector3 targetPosition = _open ? OpenPosition : ClosedPosition;
        Quaternion targetRotation = _open ? OpenRotation : ClosedRotation;

        VentObject.localPosition = Vector3.Lerp(VentObject.localPosition, targetPosition, PositionSpeed * Time.deltaTime);
        VentObject.localRotation = Quaternion.Lerp(VentObject.localRotation, targetRotation, RotationSpeed * Time.deltaTime);
    }
}
