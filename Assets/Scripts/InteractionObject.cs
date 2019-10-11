using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    public List<InteractionOption> Options = new List<InteractionOption>();
    public string Name;
    public InteractionSystem interactionSystem;
    
    internal virtual void Start()
    {
         
    }

    private bool _dirty;
    public bool IsDirty
    {
        get { return _dirty; }
        private set { _dirty = value; }
    }

    public virtual void OnInteraction(InteractionSystem system, int option = 0)
    {
        List<InteractionOption> ActiveOptions = Options.Where(a => a.Validate == null ||  a.Validate.Invoke()).ToList();

        if (ActiveOptions.Count >= option)
        {
            if (ActiveOptions[option].OnInteractionEvent != null)
            {
                ActiveOptions[option].OnInteractionEvent.Invoke();

            }
            if (ActiveOptions[option].OnInteraction != null)
                ActiveOptions[option].OnInteraction();

        }

        SetDirty();
    }

    public void SetDirty()
    {
        IsDirty = true;
    }

    public void ClearDirty()
    {
        IsDirty = false;
    }

    public string[] GetOptionNames()
    {
        return Options.Select(a => a.Name).ToArray();
    }

    
}

[System.Serializable]
public struct InteractionOption
{
    public UnityEvent OnInteractionEvent;
    public Action OnInteraction;
    public Func<bool> Validate;
    public string Name;

    public void UseInteraction()
    {
        if (OnInteraction != null)
            OnInteraction();

        if (OnInteractionEvent != null)
            OnInteractionEvent.Invoke();
    }
}