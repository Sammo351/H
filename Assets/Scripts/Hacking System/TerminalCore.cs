using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerminalCore : MonoBehaviour
{
    public static TerminalCore Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TerminalCore>();
            return _instance;
        }
    }

    public static TerminalCore _instance;
    public TerminalUI _ui;

    void Awake()
    {
        _ui = GetComponent<TerminalUI>();
        EventSystem.current.SetSelectedGameObject(_ui.inputField.gameObject, null);
        _ui.inputField.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    public List<Command> Commands = new List<Command>();

    public static void RegisterCommand(Command command)
    {
        Instance.Commands.Add(command);
    }

    public static void RemoveCommand(Command command)
    {
        Instance.Commands.Remove(command);

    }

    public static void AddLoadingBar(float time, Action callback, params string[] visual)
    {
        Instance._ui.AddLoadingBar(time, callback, visual);
        PauseTerminal(time + 0.15f);
    }

    public static void PauseTerminal(float x)
    {
        Instance.GetComponent<TerminalLogic>().PauseTimer(x);
    }

    public static void ClearTerminal()
    {
        Instance._ui.ClearTerminal();
    }

    public static Command GetCommand(string command)
    {
        List<Command> matchingCommands = new List<Command>();

        foreach (Command cmd in Instance.Commands)
        {
            if (cmd.KeyWord.ToUpper() == command.ToUpper())
                matchingCommands.Add(cmd);
        }

        //Debug.Log(matchingCommands.Count);

        if (matchingCommands.Count > 0)
            return matchingCommands[0];

        return null;
    }

    public static void AddMessage(string message, bool newLine = true)
    {
        Instance._ui.AddMessage(message, newLine);
    }

    public static string[] GetClosestMatches(string key, params string[] list)
    {
        if (list == null || list.Length == 0)
            return null;

        string[] starting = GetListOfStartingWith(key, list);
        List<string> sortedMatches = starting.ToList();
        sortedMatches.Sort();

        return sortedMatches.ToArray();
    }

    public static string[] GetListOfStartingWith(string start, params string[] list)
    {
        return list.Where(a => a.StartsWith(start, true, System.Globalization.CultureInfo.CurrentCulture)).ToArray();
    }

    internal static CachedConsoleLine AddCachedMessage(string v)
    {
        return Instance._ui.AddCachedMessage(v);
    }
}
