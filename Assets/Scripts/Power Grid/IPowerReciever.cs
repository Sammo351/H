using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerReciever
{
    void OnPowerConnectionLost();
    void OnPowerConnectionEstablished();
}
