using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGrid : MonoBehaviour
{
    public List<IPowerReciever> PowerReceivers = new List<IPowerReciever>();

    public bool DefaultOnState;
    public bool IsOn = false;

    private void Start()
    {
        if (DefaultOnState)
            ConnectPowerGrid();
    }

    [ContextMenu("Turn On")]
    void TurnOn()
    {
        ConnectPowerGrid();
    }

    [ContextMenu("Turn Off")]
    void TurnOff()
    {
        DisconnectPowerGrid();
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
}
