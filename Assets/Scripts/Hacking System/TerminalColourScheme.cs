using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalColourScheme : MonoBehaviour
{
    public static TerminalColourScheme Instance
    {
        get { if (_instance == null) _instance = FindObjectOfType<TerminalColourScheme>(); return _instance; }
    }

    private static TerminalColourScheme _instance;

    public static string TEXT
    {
        get { return Instance.text; }
    }

    public static string INFO
    {
        get { return Instance.info; }
    }

    public static string WARNING
    {
        get { return Instance.warning; }
    }

    public static string DANGER
    {
        get { return Instance.danger; }
    }

    public string text, info, warning, danger;

    public static string FormatText(string text, TerminalStyle style)
    {
        string scheme = "";
        switch (style)
        {
            case TerminalStyle.TEXT: scheme = TEXT; break;
            case TerminalStyle.INFO: scheme = INFO; break;
            case TerminalStyle.WARNING: scheme = WARNING; break;
            case TerminalStyle.DANGER: scheme = DANGER; break;
        }

        return string.Format(scheme, text);
    }
}

public enum TerminalStyle { TEXT, INFO, WARNING, DANGER };
