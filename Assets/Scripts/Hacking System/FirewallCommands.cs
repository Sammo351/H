using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class FirewallCommands : MonoBehaviour
{
    public void Start()
    {
        Command analzye = new Command("analyze");
        analzye.OnProcessCommand += AnalyzeCommand;
        TerminalCore.RegisterCommand(analzye);

        Command solve = new Command("solve");
        solve.OnProcessCommand += SolveCommand;
        TerminalCore.RegisterCommand(solve);
    }

    CachedConsoleLine consoleLine;

    public void AnalyzeCommand(string[] args)
    {
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        NetworkFirewall wall = device.networkNode.FireWall;
        if (wall == null)
        {
            TerminalCore.AddMessage("\t- " + TerminalColourScheme.FormatText("No Firewall Detected.", TerminalStyle.TEXT));
            return;
        }

        NetworkFirewall.FirewallLevel level = wall.GetNextClosedFirewall();

        consoleLine = TerminalCore.AddCachedMessage("\t- Guess: " + level.CurrentGuess);
    }

    public void SolveCommand(string[] args)
    {
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        NetworkFirewall wall = device.networkNode.FireWall;
        NetworkFirewall.FirewallLevel level = wall.GetNextClosedFirewall();
        int levelIndex = wall.GetNextClosedFirewallIndex();

        if (args.Length > 0)
        {
            if (args[0].ToUpper() == level.Password.ToUpper())
            {
                TerminalCore.AddLoadingBar(0.5f, AttemptSolve, "\n\tAttempting Solution: " + level.Password, "\n\tSolution Accepted.");
                
                level.CurrentGuess = level.Password;
                return;
            }

            char guess = args[0].ToUpper()[0];
            char[] CurrentGuess = level.CurrentGuess.ToCharArray();
            for (int i = 0; i < level.Password.Length; i++)
            {
                if (level.Password.ToUpper()[i] == guess)
                {
                    CurrentGuess[i] = level.Password[i];
                }
            }

            level.CurrentGuess = new string(CurrentGuess);
            TerminalCore.AddMessage("");
            string[] array = level.CurrentGuess.ToCharArray().Select(a => a.ToString()).ToArray();
            array[0] = "\t - Guess: " + array[0];
            TerminalCore.AddLoadingBar(0.5f, AttemptSolve, array);
        }

    }

    public void AttemptSolve()
    {
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        NetworkFirewall wall = device.networkNode.FireWall;
        NetworkFirewall.FirewallLevel level = wall.GetNextClosedFirewall();
        int levelIndex = wall.GetNextClosedFirewallIndex();

        if (level.CurrentGuess == level.Password)
        {
            level.Unlocked = true;

                ModifyLevel(levelIndex, true);

            TerminalCore.AddMessage(
                                    string.Format("\t- Firewall Cracked:" + TerminalColourScheme.FormatText("{0}/{1}", TerminalStyle.INFO),
                                    wall.GetSolvedLevels(),
                                    wall.GetTotalLevels()
                                    ));


            if (wall.GetSolvedLevels() == wall.GetTotalLevels())
                TerminalCore.AddMessage(TerminalColourScheme.FormatText("\n\t- Firewall Broken.", TerminalStyle.WARNING) + TerminalColourScheme.FormatText("\n\tAccess Granted.", TerminalStyle.INFO));

        }
        else
        {
            TerminalUI.ForceInput("solve ");
        }
    }

  
    public void ModifyLevel(int level, bool open)
    {
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        NetworkFirewall wall = device.networkNode.FireWall;
        wall.Level[level].Unlocked = open;
    }

    public void CrackFirewall()
    {
        TerminalCore.PauseTerminal(0.3f);
        NetworkDevice device = TerminalNetwork.GetCurrentDevice();
        NetworkFirewall wall = device.networkNode.FireWall;
        NetworkFirewall.FirewallLevel level = wall.GetNextClosedFirewall();
        if (!IsCracked(level.CurrentGuess))
        {
            int r = GetRandomLetter(level.CurrentGuess);
            char[] guess = new char[level.CurrentGuess.Length];

            for (int i = 0; i < level.CurrentGuess.Length; i++)
            {
                if (i == r)
                    guess[i] = level.Password[i];
                else
                    guess[i] = level.CurrentGuess[i];
            }

            level.CurrentGuess = new string(guess);
        }

        consoleLine.ConsoleLine = "\t- Guess: " + TerminalColourScheme.FormatText(level.CurrentGuess, TerminalStyle.INFO);
        consoleLine = null;
    }



    public int GetRandomLetter(string s)
    {
        int i = Random.Range(0, s.Length);
        if (s[i] == '*')
            return i;
        else
            return GetRandomLetter(s);
    }

    public bool IsCracked(string s)
    {
        for (int i = 0; i < s.Length; i++)
            if (s[i] == '*')
                return false;

        return true;
    }
}
