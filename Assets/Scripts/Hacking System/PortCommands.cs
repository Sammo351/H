using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PortCommands : MonoBehaviour
{
    public void Start()
    {
        Command portQuery = new Command("PortQuery");
        portQuery.OnProcessCommand += PortQuery;
        TerminalCore.RegisterCommand(portQuery);

        Command porthack = new Command("porthack");
        porthack.OnProcessCommand += PortHackCommand;
        TerminalCore.RegisterCommand(porthack);
    }


    public void PortQuery(string[] args)
    {
        var device = TerminalNetwork.GetCurrentDevice();
        List<string> ports = new List<string>();

        for (int j = 0; j < device.networkNode.Ports.Count; j++)
        {
            int portNumber = device.networkNode.Ports[j].portNumber;
            bool open = device.networkNode.Ports[j].Open;
            ports.Add(TerminalColourScheme.FormatText(portNumber.ToString().PadRight(3), TerminalStyle.INFO) + " - " + TerminalColourScheme.FormatText(open.ToString(), TerminalStyle.WARNING));
        }

        for (int i = 0; i < ports.Count; i++)
        {
            string formatted = string.Format("\tPort: {0}", ports[i]);
            TerminalCore.AddMessage(formatted);
        }

    }
    int portToOpen = 0;

    private void PortHackCommand(string[] args)
    {
        if (args.Length == 0)
            return;

        int port = int.Parse(args[0]);
        portToOpen = port;
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        TerminalCore.AddLoadingBar(2f, OpenPort, "\n\tPort:" + port + " -", "-", "-", "-", "-", "-", "-", "-", "Cracked.");

    }

    void OpenPort()
    {
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        TerminalCore.AddMessage("\t" + portToOpen + " - Access Granted.");
            device.networkNode.OpenPort(portToOpen);
    }

}
