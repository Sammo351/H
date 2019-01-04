using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using UnityEngine.UI;


public class Keypad : NetworkDevice, IPowerReciever
{
    public UnityEvent OnCodeSuccessful;
    public UnityEvent OnCodeUnsuccessful;
    public UnityEvent OnTimeOut;

    public string Code;
    public string CurrentCode = "";

    public Text IndicatorText;

    public Image PanelIndicator;

    public AudioSource audioSource;

    [Header("Timer Settings")]
    public bool EnableTimer;
    public float Timer = 3f;
    private float _timer;
    private bool startTimer = false;

    [Header("Power Settings")]
    public bool HasPower = false;
    public PowerGrid powerGrid;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        OnPowerConnectionLost();

        if (powerGrid)
            powerGrid.ConnectRecieverToGrid(this);

        Command unlock = new Command("Unlock");
        unlock.OnProcessCommand += UnlockDoor;
        DeviceCommands.Add(unlock);
    }

    public void UnlockDoor(string[] args)
    {
        if (!HasPower && powerGrid != null)
            return;

        if (!networkNode.IsPortOpen(3389))
        {
            TerminalCore.AddMessage(TerminalPresetMessages.PortsRestricted);
            return;
        }

        NotifySuccessful();

        if (PanelIndicator != null)
            PanelIndicator.material.color = Color.red;

        IndicatorText.text = "HACK";
    }

    public void PressButton(string digit)
    {
        if (!HasPower && powerGrid != null)
            return;

        CurrentCode += digit;
        UpdateText();
        if (audioSource)
            audioSource.Play();
    }

    public void AttemptCode()
    {
        if (!HasPower && powerGrid != null)
            return;

        if (CurrentCode == Code)
            NotifySuccessful();
        else
            NotifyUnsuccessful();
    }

    public void ClearCode()
    {
        if (!HasPower && powerGrid != null)
            return;

        CurrentCode = "";
        UpdateText();
    }

    void UpdateText()
    {
        IndicatorText.text = CurrentCode.PadRight(4, '*');
    }

    private void Update()
    {
        if (_timer > 0 && startTimer)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                OnTimeOut.Invoke();
                startTimer = false;
            }
        }
    }

    [ContextMenu("Success")]
    public void NotifySuccessful() { if (OnCodeSuccessful != null) { OnCodeSuccessful.Invoke(); _timer = Timer; startTimer = true; } }

    [ContextMenu("Failure")]
    public void NotifyUnsuccessful() { if (OnCodeUnsuccessful != null) OnCodeUnsuccessful.Invoke(); }

    public void OnPowerConnectionLost()
    {
        HasPower = false;
        animator.SetBool("On", false);
    }

    public void OnPowerConnectionEstablished()
    {
        animator.SetBool("On", true);
        HasPower = true;
    }
}
