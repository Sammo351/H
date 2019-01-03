using UnityEngine;
using UnityEngine.Networking;

public class ShellCommand : MonoBehaviour
{
    public void Start()
    {
        Command shell = new Command("Shell");
        shell.OnProcessCommand += Shell;
        TerminalCore.RegisterCommand(shell);
    }

    public void Shell(string[] args)
    {
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();

        if (device == null)
            return;

        if (args.Length == 0)
        {
            TerminalCore.AddMessage("<color=#0087ff> Shell <cmd>:</color>");

            foreach (string cmd in device.GetCommands())
            {
                TerminalCore.AddMessage("\t-" + cmd);
            }
        }
        else
        {
            string inputString = args[0];
            for (int i = 1; i < args.Length; i++)
                inputString += " " + args[i];

            Command command = device.GetCommand(TerminalLogic.FindKeyword(inputString));
            if (command != null)
            {
                TerminalCore.AddLoadingBar(0.4f, null, "\n\tSending Command.", "\n\tCommand Recieved");

                Shell(inputString);
            }
            else
                PrintUsage();

        }
    }

    
    void Shell(string args)
    {
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        device.ProcessCommand(args);
    }

    public void PrintUsage()
    {
        TerminalCore.AddMessage("\t Shell <cmd>:");
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();

        foreach (string cmd in device.GetCommands())
        {
            TerminalCore.AddMessage("\t -" + cmd);
        }
    }
}