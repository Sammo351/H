using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TerminalCore))]
public class TerminalLogic : MonoBehaviour
{
    public List<string> AutoCompleteList = new List<string>();
    private List<string> _defaultAutoComplete = new List<string>();

    public delegate void DelegateVoid();
    public DelegateVoid OnAutoCompleteListReset;


    private Queue<string> inputQueue = new Queue<string>();

    private void Start()
    {
        AddDefaultAutoCompleteList();
        OnAutoCompleteListReset += AddDefaultAutoCompleteList;
    }

    public void AddStringToDefaultAutoComplete(string text)
    {
        _defaultAutoComplete.Add(text);
    }

    public void ClearAutoCompleteList()
    {
        AutoCompleteList.Clear();
        if (OnAutoCompleteListReset != null)
            OnAutoCompleteListReset();

        AddDefaultAutoCompleteList();
    }

    private void AddDefaultAutoCompleteList()
    {
        foreach (Command command in TerminalCore.Instance.Commands)
        {
            AutoCompleteList.Add(command.KeyWord);
        }
    }

    public static void AddStringToAutocomplete(string s)
    {
        FindObjectOfType<TerminalLogic>().AutoCompleteList.Add(s);
    }

    public static void RemoveStringFromAutocomplete(string s)
    {
        FindObjectOfType<TerminalLogic>().AutoCompleteList.Remove(s);
    }


    public string AttemptAutoComplete(string input)
    {
        string[] matches = TerminalCore.GetClosestMatches(input, AutoCompleteList.ToArray());
        
        if (matches.Length == 1)
            return matches[0];
        else
        {
            int printPerLine = 5;
            for (int i = 0; i < matches.Length; i += printPerLine)
            {
                string message = matches[i];
                for (int j = 1; j < printPerLine; j++)
                {
                    if (i + j < matches.Length)
                        message += "          |        " + matches[i + j];
                }


                //TerminalCore.AddMessage(message);
            }

            return matches[0];
        }

        return null;
    }

    public void OnInputRecieved(string input)
    {
        //inputQueue.Enqueue(input);

        if (input != null && input != "")
        {
            Command command = TerminalCore.GetCommand(FindKeyword(input));
            if (command != null)
            {
                TerminalUI.usedCommandIndex = -1;
                TerminalUI.usedCommands.Push(input);
            }
        }

        if (pauseTimer > 0)
        {
            inputQueue.Enqueue(input);
            return;
        }

        if (inputQueue.Count > 0)
        {
            inputQueue.Enqueue(input);
            return;
        }

        ProcessCommand(input);

    }

    void ProcessCommand(string input)
    {
        //storing commands, for up/down recall


        Command command = TerminalCore.GetCommand(FindKeyword(input));
        string[] args = GetArgs(input);
        List<string> lst = args.ToList();
        lst.RemoveAt(0);
        args = lst.ToArray();

        Debug.Log(input);
        if (command != null)
        {
            if (command.OnProcessCommand != null)
            {
                command.OnProcessCommand(args);
            }
        }
    }

    void Update()
    {
        if (pauseTimer > 0)
            pauseTimer -= Time.deltaTime;

        if (pauseTimer < 0)
            pauseTimer = 0f;

        if (pauseTimer == 0 && inputQueue.Count > 0)
        {
            ProcessCommand(inputQueue.Dequeue());
        }
    }

    private float pauseTimer = 0f;

    public void PauseTimer(float x)
    {
        pauseTimer = x;
    }

    public static string FindKeyword(string input)
    {
        string[] split = input.Split(' ', '.');

        if (split.Length > 0)
            return split[0];

        return null;
    }

    public static string[] GetArgs(string input)
    {
        string[] split = input.Split(' ');

        split.ToList().RemoveAt(0);

        return split.ToArray();
    }
}
