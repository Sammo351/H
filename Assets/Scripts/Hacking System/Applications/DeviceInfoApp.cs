using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeviceInfoApp : MonoBehaviour
{

    public NetworkDevice Device;
    public TextMeshProUGUI text;

    public void Update()
    {
        text.text = "";
        Device = TerminalNetwork.GetCurrentDevice();
        if (Device == null)
            return;

        NetworkNode node = Device.networkNode;

        AddText("Name: " + Device.GetDeviceName());

        if (node == null)
            return;

        if (node.PasswordProtected)
            AddText(TerminalColourScheme.FormatText("Password: Protected", TerminalStyle.DANGER));

        if (node.Ports.Count > 0)
        {
            AddText(TerminalColourScheme.FormatText("Ports:", TerminalStyle.INFO));
            for (int i = 0; i < node.Ports.Count; i++)
            {
                string color = node.Ports[i].Open ? "<color=green><u><b>" : "<color=red>";
                AddText("\t -" + node.Ports[i].portNumber + " : " + color + node.Ports[i].Open.ToString() + "</color></u></b>");
            }
        }

        if (node.FireWall != null)
        {
            string s = TerminalColourScheme.FormatText("Firewall Detected!", TerminalStyle.DANGER);
            AddText(s);
        }
        else
        {
            string s = TerminalColourScheme.FormatText("No Firewall Detected.", TerminalStyle.INFO);
            AddText(s);
        }

        if (Device.DeviceCommands.Count > 0)
        {
            AddText(TerminalColourScheme.FormatText("CMDS:", TerminalStyle.INFO));

            if (node.IsPortOpen(3389))
            {
                for (int i = 0; i < Device.DeviceCommands.Count; i++)
                {
                    Debug.Log(i);
                    AddText("\t -" + Device.DeviceCommands[i].KeyWord);

                    if(Device.DeviceCommands[i].SubCommands != null)
                    {
                    for (int j = 0; j < Device.DeviceCommands[i].SubCommands.Length; j++)
                        AddText("\t\t -<" + Device.DeviceCommands[i].SubCommands[j] + ">");
                    }
                }
            }
            else
                AddText(TerminalPresetMessages.PortsRestricted);
        }

        AddText("<b><u> Details: </b></u>");
        AddText(Device.GetData());
    }

    void AddText(string s, bool newLine = true)
    {
        text.text += s;
        if (newLine)
            text.text += "\n";
    }

}
