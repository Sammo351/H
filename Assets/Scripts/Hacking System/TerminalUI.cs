using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections;

[RequireComponent(typeof(TerminalLogic))]
public class TerminalUI : MonoBehaviour
{
    TerminalLogic _logic;
    public TextMeshProUGUI outputText;
    public InputField inputField;
    public static int usedCommandIndex = 0;
    public static Stack<string> usedCommands = new Stack<string>();
    private void Start()
    {
        _logic = GetComponent<TerminalLogic>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            string autocorrected = _logic.AttemptAutoComplete(inputField.text);

            if (autocorrected != null)
            {
                SetInputField(autocorrected);
            }

            AddMessage(autocorrected);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            usedCommandIndex = Mathf.Clamp(usedCommandIndex + 1, -1, usedCommands.Count - 1);
            string usedCommand = usedCommandIndex >= 0 ? usedCommands.ElementAt(usedCommandIndex) : "";
            SetInputField(usedCommand);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            usedCommandIndex = Mathf.Clamp(usedCommandIndex - 1, -1, usedCommands.Count - 1);
            string usedCommand = usedCommandIndex >= 0 ? usedCommands.ElementAt(usedCommandIndex) : "";
            SetInputField(usedCommand);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            usedCommandIndex = -1;
            string usedCommand = "";
            SetInputField(usedCommand);
        }

    }
    public void SetInputField(string text)
    {
        inputField.text = text;
        inputField.selectionAnchorPosition = inputField.text.Length;
        inputField.selectionFocusPosition = inputField.text.Length;
    }
    internal void AddLoadingBar(float time, Action callback, params string[] visual)
    {
        StartCoroutine(HandleLoadingBar(time, callback, visual));
    }


    private IEnumerator HandleLoadingBar(float time, Action callback, params string[] visual)
    {
        // CachedConsoleLine consoleLine = TerminalCore.AddCachedMessage(visual[0]);

        for (int i = 0; i < visual.Length; i++)
        {
            outputText.text += visual[i];
            yield return new WaitForSeconds((float)time / (float)visual.Length);
        }

        if (callback != null)
            callback();

        yield return null;
    }




    internal void ClearTerminal()
    {
        outputText.text = string.Empty;
    }

    public void OnInputChanged(string changed)
    {
        _logic.OnInputRecieved(changed);
        inputField.text = string.Empty;
        inputField.Select();
        inputField.ActivateInputField();
        usedCommandIndex = -1;
    }

    internal static void ForceInput(string v)
    {
        FindObjectOfType<TerminalUI>().inputField.text = v;
        //FindObjectOfType<TerminalUI>().inputField.selectionAnchorPosition = v.Length;
        FindObjectOfType<TerminalUI>().inputField.caretPosition = v.Length;
    }

    public void AddMessage(string text, bool newLine = true)
    {
        if(newLine)
            outputText.text += "\n";
        outputText.text += text;
        string[] split = outputText.text.Split('\n');

        if (split.Length > 64)
        {
            List<string> lst = split.ToList();
            lst.RemoveRange(0, split.Length - 64);
            outputText.text = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i] != string.Empty)
                {
                    outputText.text += '\n' + lst[i];
                }
            }

        }
    }

    List<CachedConsoleLine> CachedConsoleLines = new List<CachedConsoleLine>();

    public CachedConsoleLine AddCachedMessage(string text)
    {
        outputText.text += "\n" + text;
        

        string[] split = outputText.text.Split('\n');
        int lineNumber = split.Length-1;

        if (split.Length > 64)
        {
            List<string> lst = split.ToList();
            int delta = split.Length - 64;
            lst.RemoveRange(0, delta);
            outputText.text = "";

            lineNumber -= delta;

            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i] != string.Empty)
                    outputText.text += '\n' + lst[i];
            }

            for (int i = 0; i < CachedConsoleLines.Count; i++)
                CachedConsoleLines[i].lineNumber -= delta;
        }

        CachedConsoleLine consoleLine = new CachedConsoleLine(lineNumber, text);
        CachedConsoleLines.Add(consoleLine);
        return consoleLine;
    }

    internal static void UpdateConsoleLine(CachedConsoleLine cachedConsoleLine)
    {
        TerminalUI ui = FindObjectOfType<TerminalUI>();
        string[] lines = ui.outputText.text.Split('\n');

        if (!cachedConsoleLine.IsAlive())
            return;

        if (cachedConsoleLine.lineNumber > lines.Length)
            return;

        Debug.Log(cachedConsoleLine.lineNumber);
        lines[cachedConsoleLine.lineNumber] = cachedConsoleLine.ConsoleLine;
        string text = lines[0];

        for (int i = 1; i < lines.Length; i++)
            text += "\n" + lines[i];

        FindObjectOfType<TerminalUI>().outputText.text = text;
    }
    public static string ColorText(string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }
}

public class CachedConsoleLine
{
    public int lineNumber;
    private string _consoleLine;

    public CachedConsoleLine(int lineNumber, string consoleLine)
    {
        this.lineNumber = lineNumber;
        this._consoleLine = consoleLine;
    }

    public string ConsoleLine
    {
        get
        {
            return _consoleLine;
        }
        set
        {
            _consoleLine = value;
            TerminalUI.UpdateConsoleLine(this);
        }
    }

    public bool IsAlive()
    {
        if (lineNumber < 0)
            return false;

        return true;
    }

}