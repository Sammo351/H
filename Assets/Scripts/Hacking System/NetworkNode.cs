using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkNode : MonoBehaviour
{
    //private Dictionary<int, bool> _ports = new Dictionary<int, bool>();
    public List<Port> Ports = new List<Port>();
    public List<NetworkNode> _nodes = new List<NetworkNode>();
    public NetworkDevice networkDevice;
    public NetworkFirewall FireWall;

    [System.Serializable]
    public struct Port { public int portNumber; public bool Open; }

    public string Password;
    private bool passwordLocked;

    void Start()
    {
        if (Password != null)
            passwordLocked = true;

        networkDevice = GetComponent<NetworkDevice>();
        for(int i = 0; i < _nodes.Count; i++)
        {
            if(!_nodes[i]._nodes.Contains(this))
            {
                _nodes[i]._nodes.Add(this);
            }
        }
    }

    public int RequiredPorts = 1;

    public bool PasswordProtected
    {
        get
        {
            return Password != String.Empty;
        }
    }

    public bool IsPortOpen(int port)
    {
        int _p = FindPort(port);
        if (_p == -1)
            return true;

        Port P = Ports.ElementAt(_p);
        return P.Open;
    }

    public bool OpenPort(int port)
    {
        return ModifyPort(port, true);
    }

    bool ModifyPort(int port, bool state)
    {

        int _p = FindPort(port);
        if (_p == -1)
            return false;

        Port P = Ports.ElementAt(_p);
        P.Open = state;
        Ports[_p] = P;

        return true;
    }

    public bool ClosePort(int port)
    {
        return ModifyPort(port, false);
    }

    public int GetOpenPortCount()
    {
        int i = 0;

        foreach (Port port in Ports)
            if (port.Open)
                i++;

        return i;
    }

    private int FindPort(int portNumber)
    {
        for (int i = 0; i < Ports.Count; i++)
        {
            if (Ports[i].portNumber == portNumber)
                return i;
        }
        return -1;
    }

    private void SetPortObject(Port port)
    {


    }

    public bool HasAccess(out string reason)
    {
        reason = "Access Denied. ";
        if (GetOpenPortCount() < RequiredPorts)
        {
            reason += "Ports Blocked.";
            return false;
        }

        if (PasswordProtected && passwordLocked)
        {
            reason += "Password Locked.";
            return false;
        }

        if (FireWall != null && !FireWall.IsOpen())
        {
            reason += "Firewall active.";

            return false;
        }

        return true;
    }

    public bool HasAccess()
    {
        string s;
        return HasAccess(out s);
    }

    internal bool AttemptLogin(string v)
    {
        if (GetOpenPortCount() < RequiredPorts)
        {
            TerminalCore.AddMessage(TerminalColourScheme.FormatText("\t- Access Denied. Ports Blocked", TerminalStyle.DANGER));
            return false;
        }

        if (PasswordProtected && Password == v)
        {
            passwordLocked = false;
            return true;
        }

        return false;
    }

    public void OnDrawGizmos()
    {
        for(int i = 0; i < _nodes.Count; i++)
        {
            Gizmos.color = Color.yellow;    
            Gizmos.DrawLine(transform.position, _nodes[i].transform.position);
        }
    }
}