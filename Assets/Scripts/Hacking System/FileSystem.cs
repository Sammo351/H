using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSystem : MonoBehaviour
{

    NetworkDevice device;
    const int FileSystemPort = 255;

    public FSDirectory RootDrive = new FSDirectory("C:");
    public FSDirectory CurrentDirectory;

    void Start()
    {
        device = GetComponent<NetworkDevice>();
        CurrentDirectory = RootDrive;
        SetParentsForFiles(RootDrive);
    }

    public List<FSFile> SetParentsForFiles(FSDirectory directory)
    {
        List<FSFile> files = new List<FSFile>();
        files.AddRange(directory.Files);
        for (int i = 0; i < files.Count; i++)
        {
            files[i].Parent = directory;
        }

        for (int i = 0; i < directory.Directories.Count; i++)
        {
            directory.Directories[i].Parent = directory;
            files.AddRange(SetParentsForFiles(directory.Directories[i]));
        }

        return files;
    }

    public static string GetFilePath(FSFile fS)
    {
        FSDirectory dir = fS.Parent;
        string text = fS.Name;
        while (dir != null)
        {
            text = dir.Name + "/" + text;
            dir = dir.Parent;
        }

        return text;
    }

    public static string GetDirectoryPath(FSDirectory fS)
    {
        FSDirectory dir = fS.Parent;
        string text = fS.Name;
        while (dir != null)
        {
            text = dir.Name + "/" + text;
            dir = dir.Parent;
        }

        return text;
    }

    public static int GetChildIndex(FSDirectory dir)
    {
        FSDirectory cur = dir;
        int index = 0;
        while (cur.Parent != null)
        {
            index++;
            cur = cur.Parent;
        }
        return index;
    }

    public static int GetChildIndex(FSFile dir)
    {
        FSDirectory cur = dir.Parent;
        int index = 1;
        while (cur.Parent != null)
        {
            index++;
            cur = cur.Parent;
        }
        return index;
    }
    public static int GetParentCount(FSDirectory fd)
    {
        int count =0;
        FSDirectory current = fd.Parent;
        while(current != null)
        {
            current = current.Parent;
            count++;
        }
        return count;
    }
   

    public bool HasPermission()
    {
        if (device.networkNode.HasAccess())
        {
            if (device.networkNode.IsPortOpen(FileSystemPort))
                return true;
        }

        return false;
    }
}
[System.Serializable]
public class FSDirectory
{
    public string Name;
    public FSDirectory(string name)
    {
        this.Name = name;
    }

    public List<FSDirectory> Directories = new List<FSDirectory>();
    public List<FSFile> Files = new List<FSFile>();
    [System.NonSerialized]
    public FSDirectory Parent;

    public int GetChildren()
    {
        return Directories.Count + Files.Count;
    }
}

[System.Serializable]
public class FSFile
{
    public string Name;
    public FSFile(string name, FileType fileType = FileType.Text)
    {
        this.Name = name;
        this.fileType = fileType;
    }

    public enum FileType { Text, Image, Application };
    public FileType fileType;

    [Multiline]
    public string Data;

    [HideInInspector]
    public FSDirectory Parent;


}