using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class NetworkFirewall : MonoBehaviour
{
    public FirewallLevel[] Level;

    public void Start()
    {
        foreach (FirewallLevel level in Level)
        {
            int length = level.Password.Length;
            level.CurrentGuess = new string('*', length);
        }
    }

    

    [System.Serializable]
    public class FirewallLevel
    {
        public string Password = "Fallafel";
        public string CurrentGuess = "********";
        public bool Unlocked = false;
    }

    public bool IsOpen()
    {
        bool isOpen = true;
        for (int i = 0; i < Level.Length; i++)
            if (!Level[i].Unlocked)
                isOpen = false;

        return isOpen;
    }

    public FirewallLevel GetNextClosedFirewall()
    {
        for (int i = 0; i < Level.Length; i++)
        {
            if (!Level[i].Unlocked)
                return Level[i];
        }

        return null;
    }

    public int GetNextClosedFirewallIndex()
    {
        for (int i = 0; i < Level.Length; i++)
        {
            if (!Level[i].Unlocked)
                return i;
        }

        return -1;
    }

    public int GetSolvedLevels()
    {
        int solved = 0;
        for (int i = 0; i < Level.Length; i++)
            if (Level[i].Unlocked)
                solved++;

        return solved;
    }

    public int GetTotalLevels()
    {
        return Level.Length;
    }
}

