using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    public UnityEvent OnInteractionEvent;

    public virtual void OnInteraction(InteractionSystem system)
    {
        if (OnInteractionEvent != null)
            OnInteractionEvent.Invoke();
    }


}