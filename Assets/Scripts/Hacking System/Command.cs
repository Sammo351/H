using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public Command(string KeyWord)
    {
        this.KeyWord = KeyWord;
    }

    public string KeyWord;
    public string[] SubCommands;
    public delegate void CommandDelegate(string[] args);
    public CommandDelegate OnProcessCommand;

}
