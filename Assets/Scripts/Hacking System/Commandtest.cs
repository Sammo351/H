using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commandtest : MonoBehaviour
{

    private void Start()
    {
        Command echoCommand = new Command("Echo");
        echoCommand.OnProcessCommand += PrintCommand;
        TerminalCore.RegisterCommand(echoCommand);

        Command clearCommand = new Command("Clear");
        clearCommand.OnProcessCommand += ClearConsole;
        TerminalCore.RegisterCommand(clearCommand);
    }

    public void PrintCommand(string[] args)
    {
        Debug.Log(args);

        for (int i = 0; i < args.Length; i++)
        {
            bool newLine = i == 0;
            TerminalCore.AddMessage(args[i] + " ", newLine);
        }
    }

    public void ClearConsole(string[] args)
    {
        TerminalCore.ClearTerminal();
    }
}
