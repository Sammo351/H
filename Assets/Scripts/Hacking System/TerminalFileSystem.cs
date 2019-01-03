using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalFileSystem : MonoBehaviour
{
    public FSDirectory CurrentDirectory;
    public List<FSFile> Files = new List<FSFile>();

    private static TerminalFileSystem Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TerminalFileSystem>();

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    
    private static TerminalFileSystem _instance;
}
