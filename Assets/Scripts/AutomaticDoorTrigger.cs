using UnityEngine;
using UnityEngine.Events;

public class AutomaticDoorTrigger : MonoBehaviour
{
    public int ItemsInBox = 0;
    public AutomaticDoor[] Doors;


    private void OnTriggerEnter(Collider other)
    {
        ItemsInBox++;
        ControlDoors();
    }

    private void OnTriggerExit(Collider other)
    {
        ItemsInBox--;
        ControlDoors();
    }

    void ControlDoors()
    {
        if (ItemsInBox > 0)
        {
            foreach (AutomaticDoor door in Doors)
            {
                door.Unlock();
                door.Open();
            }
        }
        else
        {

            foreach (AutomaticDoor door in Doors)
            {
                door.CloseWithDelay(1f);
                door.Lock();
            }
        }
    }



}