using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBox : MonoBehaviour
{
    public UnityEvent EnterTrigger;
    public UnityEvent ExitTrigger;
    public UnityEvent InTrigger;

    public int ItemsInBox = 0;


    private void OnTriggerEnter(Collider other)
    {
        EnterTrigger.Invoke();
        ItemsInBox++;
    }

    private void OnTriggerExit(Collider other)
    {
        ExitTrigger.Invoke();
        ItemsInBox--;
    }

    void Update()
    {
        if (ItemsInBox > 0)
            InTrigger.Invoke();
    }




}
