using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGrid : MonoBehaviour
{
    public List<IPowerReciever> PowerReceivers = new List<IPowerReciever>();

    public bool DefaultOnState;
    public bool IsOn = false;

    public AudioClip PowerOff;
    private AudioSource _audioSource;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (DefaultOnState)
            ConnectPowerGrid();
    }

    [ContextMenu("Turn On")]
    void TurnOn()
    {
        ConnectPowerGrid();
        _audioSource.pitch = -1;
        _audioSource.PlayOneShot(PowerOff);

    }

    [ContextMenu("Turn Off")]
    void TurnOff()
    {
        DisconnectPowerGrid();
        _audioSource.pitch = 1;
        _audioSource.PlayOneShot(PowerOff);
    }

    public void ConnectRecieverToGrid(IPowerReciever reciever)
    {
        if (IsOn)
            reciever.OnPowerConnectionEstablished();
        else
            reciever.OnPowerConnectionLost();

        if (!PowerReceivers.Contains(reciever))
            PowerReceivers.Add(reciever);
    }

    public void DisconnectRecieverFromGrid(IPowerReciever reciever)
    {
        reciever.OnPowerConnectionLost();

        if (PowerReceivers.Contains(reciever))
            PowerReceivers.Remove(reciever);
    }

    public void DisconnectPowerGrid()
    {
        IsOn = false;
        PowerReceivers.ForEach(a => a.OnPowerConnectionLost());
    }

    public void ConnectPowerGrid()
    {
        IsOn = true;
        PowerReceivers.ForEach(a => a.OnPowerConnectionEstablished());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (IsOn)
                TurnOff();
            else
                TurnOn();
        }
    }
}
