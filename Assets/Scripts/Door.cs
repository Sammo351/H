using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : InteractionObject
{
    [FormerlySerializedAs("VentObject")]
    public Transform DoorObject;

    public Vector3 OpenPosition;
    public Vector3 ClosedPosition;

    public Quaternion OpenRotation;
    public Quaternion ClosedRotation;

    public float PositionSpeed = 5f;
    public float RotationSpeed = 5f;

    public Door[] SubDoors;
    public bool Locked = false;

    internal bool CanMove = true;


    private AudioSource _audioSource;
    public AudioClip[] OpenSounds;
    public AudioClip[] ClosedSounds;

    public AudioClip BashDoorClip;

    internal override void Start()
    {
        base.Start();
        Options.Add(new InteractionOption() { Name = "Open Door", OnInteraction = Open, Validate = CanOpen });
        Options.Add(new InteractionOption() { Name = "Close Door", OnInteraction = Close, Validate = CanClose });
        Options.Add(new InteractionOption() { Name = "Break Down Door", OnInteraction = SlamOpen, Validate = () => (CanKickIn()) });
    }



    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    [ContextMenu("Goto Open")]
    void GotoOpen()
    {
        DoorObject.localPosition = this.OpenPosition;
        DoorObject.localRotation = this.OpenRotation;
    }

    [ContextMenu("Goto Closed")]
    void GotoClosed()
    {
        DoorObject.localPosition = this.ClosedPosition;
        DoorObject.localRotation = this.ClosedRotation;
    }


    [ContextMenu("Set Open")]
    void SetOpen()
    {
        this.OpenPosition = DoorObject.localPosition;
        this.OpenRotation = DoorObject.localRotation;
    }

    [ContextMenu("Set Closed")]
    void SetClosed()
    {
        this.ClosedPosition = DoorObject.localPosition;
        this.ClosedRotation = DoorObject.localRotation;
    }

    bool _open = false;

    public bool IsOpen() { return _open; }

    public override void OnInteraction(InteractionSystem system, int option)
    {
        base.OnInteraction(system, option);
    }

    public void Open()
    {
        if (!CanBeUsed())
            return;


        if (!_open && _audioSource != null && OpenSounds.Length > 0)
            _audioSource.PlayOneShot(OpenSounds[UnityEngine.Random.Range(0, OpenSounds.Length - 1)]);

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


        foreach (Door d in SubDoors)
            d._open = false;

        if (_open && _audioSource != null && ClosedSounds.Length > 0)
        {
            _audioSource.PlayOneShot(ClosedSounds[UnityEngine.Random.Range(0, ClosedSounds.Length - 1)]);
        }

        _open = false;
    }

    public void ForceOpen()
    {
        Open();
    }

    public void ForceClosed()
    {
        Close();
    }

    public void SlamOpen()
    {
        _open = true;
        _audioSource.PlayOneShot(BashDoorClip);
        RotationSpeed *= 3f;
        Unlock();
        Invoke("ResetSpeed", 1f);
    }

    private void ResetSpeed()
    {
        RotationSpeed /= 3f;
    }

    public void SlamClosed()
    {

    }


    public void Lock() { Locked = true; }
    public void Unlock() { Locked = false; }

    internal virtual bool CanOpen() { return !IsOpen() && !Locked; }
    internal virtual bool CanClose() { return IsOpen() && !Locked; }

    internal virtual bool CanKickIn()
    {
        if(Vector3.Dot(transform.forward, interactionSystem.transform.forward) > 0)
        {
            if(!IsOpen())
                return true;
        }
        return false;
    }


    private void Update()
    {
        Vector3 targetPosition = _open ? OpenPosition : ClosedPosition;
        Quaternion targetRotation = _open ? OpenRotation : ClosedRotation;

        if (CanMove)
        {
            DoorObject.localPosition = Vector3.Lerp(DoorObject.localPosition, targetPosition, PositionSpeed * Time.deltaTime);
            DoorObject.localRotation = Quaternion.Lerp(DoorObject.localRotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }


}
