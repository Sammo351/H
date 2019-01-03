using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(NetworkNode))]
public class NetworkDevice : MonoBehaviour
{
    public delegate void DeviceEvent();

    [HideInInspector]
    public NetworkNode networkNode;
    [SerializeField] string DeviceName;

    public List<Command> DeviceCommands = new List<Command>();

    void Awake()
    {
        networkNode = GetComponent<NetworkNode>();
    }

    public virtual string[] GetCommands()
    {
        string[] commands = new string[DeviceCommands.Count];
        for (int i = 0; i < DeviceCommands.Count; i++)
            commands[i] = DeviceCommands[i].KeyWord;

        return commands;
    }

    public virtual string GetDeviceName()
    {
        return DeviceName;
    }

    public virtual bool CanExecuteCommand()
    {
        return true;
    }

    public void ProcessCommand(string input)
    {
        if (!CanExecuteCommand())
            return;

        Command command = GetCommand(TerminalLogic.FindKeyword(input));
        string[] args = TerminalLogic.GetArgs(input);
        List<string> lst = args.ToList();
        lst.RemoveAt(0);
        args = lst.ToArray();

        if (command != null)
        {
            if (command.OnProcessCommand != null)
            {
                command.OnProcessCommand(args);
            }
        }
    }

    public Command GetCommand(string command)
    {
        List<Command> matchingCommands = new List<Command>();

        foreach (Command cmd in DeviceCommands)
        {
            if (cmd.KeyWord.ToUpper() == command.ToUpper())
                matchingCommands.Add(cmd);
        }

        if (matchingCommands.Count > 0)
            return matchingCommands[0];

        return null;
    }

    public virtual void OnTerminalConnected() { }

    public virtual void OnTerminalDisconnected() { }

    // Get Details to print to Details app
    public virtual string GetData()
    {
        return "";
    }

}
