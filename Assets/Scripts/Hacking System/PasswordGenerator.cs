using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PasswordGenerator : MonoBehaviour
{
    public TextAsset Password;
    public string[] Words;

    public static string GeneratePassword(int words = 2)
    {
        string[] listOfWords = FindObjectOfType<PasswordGenerator>().Words;
        string myPassword = "";
        for (int i = 0; i < words; i++)
        {
            int random = Random.Range(0, listOfWords.Length);
            myPassword += listOfWords[random];
        }

        myPassword += Random.Range(0, 2017);
        return myPassword;
    }

    public static string GenerateFirewallKey(int length)
    {
        string s = "";
        string[] correctLength = FindObjectOfType<PasswordGenerator>().Words.Where(a => a.Length == length).ToArray();
        if (correctLength.Length > 0)
            s = correctLength[Random.Range(0, correctLength.Length)];

        return s;

    }

}
