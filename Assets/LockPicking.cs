
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class LockPicking : MonoBehaviour
{
    public Transform pinPick;
    public Transform barrelPick;

    public float Threshold = 0.1f;

    public float TargetPosition = 0f;
    public float pinPickPosition = 0f;
    public float barrelPickPosition = 0f;

    public float barrelRotationSpeed = 5f;
    public float pinRotationSpeed = 5f;

    public float pinPickMin, pinPickMax, barrelPickMin, barrelPickMax;

    public float a;
    public AnimationCurve turnCurve;

    public AudioSource audioSource;

    public UnityEvent OnCompleted;
    public UnityEvent OnExit;

    public static LockPicking Instance;

    public Camera lockCamera;

    bool completed = false;
    bool isInUse = false;
    bool hasPick = false;
    public Volume volume;

    private float _pickHealth = 2f;

    private InventorySystem _userInventory;

    private void Start()
    {
        Instance = this;

    }

    public void SetupNewLock()
    {
        completed = false;
        OnCompleted.RemoveAllListeners();
        OnExit.RemoveAllListeners();
        TargetPosition = UnityEngine.Random.Range(0f, 360f);
        pinPickPosition = 0;
        barrelPickPosition = 0;

    }

    // Update is called once per frame
    private void Update()
    {
        volume.weight = Mathf.Lerp(volume.weight, isInUse ? 1f : 0f, Time.deltaTime * 3f);
        if (!isInUse)
            return;

        if (!hasPick)
            return;


        pinPickPosition += !Input.GetKey(KeyCode.D) ? Input.GetAxis("Mouse X") * Time.deltaTime * pinRotationSpeed : 0f;

        pinPickPosition = Mathf.Repeat(pinPickPosition, 360f);

        barrelPickPosition += (Input.GetKey(KeyCode.D) ? 1f : -3f) * Time.deltaTime * barrelRotationSpeed;


        a = Mathf.DeltaAngle(pinPickPosition, TargetPosition);
        //a = TargetPosition - pinPickPosition;

        a = Mathf.Abs(a);

        a = Mathf.RoundToInt(a);
        if (a <= Threshold) a = 0f;

        float maxPosition = Mathf.Lerp(0, 90f, turnCurve.Evaluate((a / 180f)));


        if (a < Threshold && (int)barrelPickPosition == 90f && !completed)
        {
            Debug.Log("Complete");
            StartCoroutine(Complete());
        }

        barrelPickPosition = Mathf.Clamp(barrelPickPosition, barrelPickMin, maxPosition);
        float barrelVisualPosition = barrelPickPosition;
        //Simulate break
        if (barrelPickPosition >= maxPosition && a != 0 && Input.GetKey(KeyCode.D))
        {
            barrelVisualPosition = barrelPickPosition + Mathf.Sin(Time.time * 30f) * 2;
            _pickHealth -= Time.deltaTime;

            if (_pickHealth <= 0)
            {
                StartCoroutine(BreakPick());
            }
        }

        var rot = barrelPick.transform.eulerAngles;
        rot.z = barrelVisualPosition;
        barrelPick.transform.eulerAngles = rot;

        rot = pinPick.transform.eulerAngles;
        rot.z = pinPickPosition;
        pinPick.transform.eulerAngles = rot;

    }

    private IEnumerator Complete()
    {
        audioSource.Play();
        completed = true;

        isInUse = false;

        yield return new WaitForSeconds(0.5f);
        lockCamera.enabled = false;
        yield return new WaitForSeconds(0.2f);

        _userInventory.AddLockPicks(1);
        if (OnCompleted != null)
            OnCompleted.Invoke();

        door.Unlock();

        if (OnExit != null)
            OnExit.Invoke();

        door = null;

    }

    Door door;
    public void ReadyGame(Door door, InventorySystem inventorySystem)
    {
        this._userInventory = inventorySystem;
        this.door = door;

        if (_userInventory.LockPicks > 0)
        {
            isInUse = true;
            lockCamera.enabled = true;
            EquipPick();
        }

    }

    public AudioClip breakPickSound;
    public AudioClip newPickSound;
    IEnumerator BreakPick()
    {
        hasPick = false;
        pinPick.GetComponentInChildren<Renderer>().enabled = false;
        audioSource.PlayOneShot(breakPickSound);
        yield return new WaitForSeconds(1f);

        if (_userInventory.LockPicks <= 0)
        {
            isInUse = false;
            door = null;
            lockCamera.enabled = false;

            if (OnExit != null)
                OnExit.Invoke();

        }
        else
            EquipPick();


    }

    void EquipPick()
    {
        if (_userInventory.LockPicks > 0 && !hasPick)
        {
            audioSource.PlayOneShot(newPickSound);
            _pickHealth = 2f;
            hasPick = true;
            _userInventory.LockPicks--;
            pinPick.GetComponentInChildren<Renderer>().enabled = true;
        }
    }

}
