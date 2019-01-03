using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class NetworkCommands : MonoBehaviour
{
    public void Start()
    {
        Command command = new Command("Scan");
        command.OnProcessCommand += ScanNetwork;
        TerminalCore.RegisterCommand(command);

        Command connect = new Command("Connect");
        connect.OnProcessCommand += ConnectNetwork;
        TerminalCore.RegisterCommand(connect);

        Command disconnect = new Command("dc");
        disconnect.OnProcessCommand += Disconnect;
        TerminalCore.RegisterCommand(disconnect);

        Command login = new Command("login");
        login.OnProcessCommand += LoginCommand;
        TerminalCore.RegisterCommand(login);


    }



    public void ScanNetwork(string[] args)
    {
        TerminalCore.AddMessage("Scanning");
        TerminalCore.AddLoadingBar(0.5f, ContinueScanning, ".", ".", ".");
    }

    private void ContinueScanning()
    {
        if (TerminalNetwork.GetCurrentDevice().networkNode.FireWall != null)
        {
            if (!TerminalNetwork.GetCurrentDevice().networkNode.FireWall.IsOpen())
            {
                TerminalCore.AddMessage(TerminalColourScheme.FormatText("\t-Access Restricted -Firewall Detected", TerminalStyle.DANGER));
                return;
            }
        }

        for (int i = 0; i < TerminalNetwork.GetCurrentDevice().networkNode._nodes.Count; i++)
        {
            NetworkNode node = TerminalNetwork.GetCurrentDevice().networkNode._nodes[i];
            string name = node.networkDevice.GetDeviceName();
            string requiredPorts = node.RequiredPorts.ToString();
            string unlocked = node.HasAccess().ToString().ToLowerInvariant();
            string firewall = node.FireWall != null ? "\n\t\t- " + TerminalColourScheme.FormatText("Firewall detected", TerminalStyle.DANGER) : "";

            unlocked = unlocked.ToLower();
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            unlocked = cultInfo.ToTitleCase(unlocked);

            string password = node.PasswordProtected.ToString();

            if (i != 0)
                TerminalCore.AddMessage("\n");

            TerminalCore.AddMessage(
                string.Format("\t<color=#0087ff>{0}</color>\n\t\t- Ports Detected: {1}\n\t\t- Open: <b>{2}</b>\n\t\t{3}{4}",
                name,
                requiredPorts,
                unlocked,
                node.PasswordProtected ? "- <color=orange>Password Protected</color>" : "- <color=#0087ff>No Password</color>",
                firewall
                ));
        }
    }

    public void ConnectNetwork(string[] args)
    {

        if (TerminalNetwork.GetCurrentDevice().networkNode.FireWall != null)
        {
            if (!TerminalNetwork.GetCurrentDevice().networkNode.FireWall.IsOpen())
            {
                TerminalCore.AddLoadingBar(0.1f, null,
              "\n \t - Attempting Connection...",
              "\n \t - Connection lost", TerminalColourScheme.FormatText("\n \t - Access Restricted - Firewall Detected", TerminalStyle.DANGER));
                //TerminalCore.AddMessage("\t-Access Restricted - Firewall Detected");
                return;
            }
        }

        TerminalCore.AddMessage("\n \t Connecting");
        if (args.Length < 1)
        {
            TerminalCore.AddLoadingBar(0.2f, null, ".", ".", ".", " \t <color=orange>Device not found</color>");
            return;
        }


        //Find Closest Match
        if (args[0].Contains("*"))
        {
            NetworkNode currentNode = TerminalNetwork.GetCurrentDevice().networkNode;

            List<NetworkNode> possibilities = currentNode._nodes.Where(a => a.networkDevice.GetDeviceName().ToUpper().StartsWith(args[0].ToUpper().Replace("*", ""))).ToList();
            possibilities.Sort();
            args[0] = possibilities[0].networkDevice.GetDeviceName();
        }

        bool deviceFound = TerminalNetwork.GetCurrentDevice().networkNode._nodes.Any(a => a.networkDevice.GetDeviceName().ToUpper() == args[0].ToUpper());
        if (deviceFound)
        {
            TerminalCore.AddLoadingBar(0.1f, AttemptConnection, ".", ".", ".");
            deviceName = args[0].ToUpper();

        }
        else
            TerminalCore.AddLoadingBar(1.5f, null, ".", ".", ".", " <color=orange>Device not found</color>");
    }


    void Connect(string name)
    {
        TerminalNetwork.ConnectTo(TerminalNetwork.GetCurrentDevice().networkNode._nodes.Single(a => a.networkDevice.GetDeviceName().ToUpper() == name).networkDevice);
    }

    string deviceName;

    private void AttemptConnection()
    {
        TerminalCore.AddLoadingBar(0.1f, CompleteConnection,
            "\n \t Attempting Connection...",
            "\n \t -<color=green>Connection Successfull. </color> \n");
    }

    private void CompleteConnection()
    {
        TerminalNetwork.ConnectTo(TerminalNetwork.GetCurrentDevice().networkNode._nodes.Single(a => a.networkDevice.GetDeviceName().ToUpper() == deviceName.ToUpper()).networkDevice);
        TerminalCore.AddMessage("<color=orange>Connected Established: " + TerminalNetwork.GetCurrentDevice().GetDeviceName() + "</color>");

        Connect(deviceName);
    }

    public void Disconnect(string[] args)
    {
        StartCoroutine(DisconnectAnim());
    }

    private IEnumerator DisconnectAnim()
    {
        CachedConsoleLine line = TerminalCore.AddCachedMessage("DC");
        yield return new WaitForSeconds(0.3f);

        line.ConsoleLine = "<color=orange>Disconnecting...</color >";
        Disconnect();
        yield return new WaitForSeconds(0.3f);

        line.ConsoleLine = "Establishing Connection...";
        yield return new WaitForSeconds(0.3f);
        line.ConsoleLine = "Connected To Host.";
    }

    void Disconnect()
    {
        TerminalNetwork.Disconnect();
        Connect(name);
    }


    public void LoginCommand(string[] args)
    {
        if (args.Length == 0)
        {
            TerminalCore.AddMessage("\t- No password input.");
            return;
        }

        if (TerminalNetwork.GetCurrentDevice().networkNode.AttemptLogin(args[0]))
        {
            TerminalCore.AddMessage("\t- Login Successful.");
            Login(args[0]);
        }
        else
            TerminalCore.AddMessage("\t- Login Failed.");
    }

    public void Login(string password)
    {
        TerminalNetwork.GetCurrentDevice().networkNode.AttemptLogin(password);
    }



}
