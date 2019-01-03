using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalFancyPants : MonoBehaviour
{
    public TextAsset bootingText;

    public void Start()
    {
        StartCoroutine(DisplayMessage());
    }

    public IEnumerator DisplayMessage()
    {
        char[] split = bootingText.text.ToCharArray();
        TerminalCore.PauseTerminal(5f);
        string[] sSplit = new string[split.Length];

        for (int i = 0; i < split.Length; i++)
        {
            sSplit[i] = split[i].ToString();
        }

        TerminalCore.AddLoadingBar(3f, null, sSplit);
        yield return new WaitForSeconds(10f);
        // TerminalCore.AddMessage("Clearing Terminal...");
        // yield return new WaitForSeconds(0.1f);
        // TerminalCore.AddMessage(".");
        // yield return new WaitForSeconds(0.1f);
        // TerminalCore.AddMessage(".");
        // yield return new WaitForSeconds(0.1f);
        // TerminalCore.AddMessage(".");
        // yield return new WaitForSeconds(1f);
        // TerminalCore.ClearTerminal();
        TerminalCore.AddMessage("System Ready");
    }

    // public IEnumerator DisplayMessage()
    // {
    //     string[] split = bootingText.text.Split('\n');
    //     TerminalCore.PauseTerminal(5f);

    //     for (int i = 0; i < split.Length; i++)
    //     {
    //         TerminalCore.AddMessage(split[i]);
    //         if (i % 10 == 0)
    //             yield return new WaitForEndOfFrame();
    //     }

    //     TerminalCore.AddMessage("Clearing Terminal...");
    //     yield return new WaitForSeconds(0.1f);
    //     TerminalCore.AddMessage(".");
    //     yield return new WaitForSeconds(0.1f);
    //     TerminalCore.AddMessage(".");
    //     yield return new WaitForSeconds(0.1f);
    //     TerminalCore.AddMessage(".");
    //     yield return new WaitForSeconds(1f);
    //     TerminalCore.ClearTerminal();
    //     TerminalCore.AddMessage("System Ready");
    // }
}
