using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalNetwork : MonoBehaviour
{
    public NetworkDevice CurrentNetworkDevice;
    public NetworkDevice RootDevice;

    private static TerminalNetwork Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TerminalNetwork>();

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private static TerminalNetwork _instance;

    public static NetworkDevice GetCurrentDevice()
    {
        return Instance.CurrentNetworkDevice;
    }

    public static void ConnectTo(NetworkDevice v)
    {
        _instance.CurrentNetworkDevice.OnTerminalDisconnected();
        _instance.CurrentNetworkDevice = v;
        _instance.CurrentNetworkDevice.OnTerminalConnected();
    }

    public static void Disconnect()
    {
        ConnectTo(_instance.RootDevice);
    }

}

