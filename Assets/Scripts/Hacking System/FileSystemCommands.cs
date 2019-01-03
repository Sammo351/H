using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FileSystemCommands : MonoBehaviour
{
    public void Start()
    {
        Command ls = new Command("ls");
        ls.OnProcessCommand += ListFiles;
        TerminalCore.RegisterCommand(ls);

        Command cd = new Command("cd");
        cd.OnProcessCommand += ChangeDirectory;
        TerminalCore.RegisterCommand(cd);

        Command print = new Command("print");
        print.OnProcessCommand += PrintCommand;
        TerminalCore.RegisterCommand(print);
    }

    public void ListFiles(string[] args)
    {
        TerminalCore.AddMessage("LS");
        string reason;

        if (!TerminalNetwork.GetCurrentDevice().networkNode.HasAccess(out reason))
        {
            TerminalCore.AddMessage(TerminalColourScheme.FormatText(reason, TerminalStyle.DANGER));
            return;
        }


        FileSystem fs = TerminalNetwork.GetCurrentDevice().GetComponent<FileSystem>();
        bool hasFileSystem = fs != null;


        print("Has File System: " + hasFileSystem);
        if (hasFileSystem)
        {
            if (fs.HasPermission())
            {
                PrintDirectory(fs.CurrentDirectory);
            }
            else
                NotifyBlocked();
        }
        else
            TerminalCore.AddLoadingBar(1.5f, null, "\nFile System: ", "Not Found.");
    }

    public void NotifyBlocked()
    {
        TerminalCore.AddMessage(TerminalColourScheme.FormatText("Access Blocked. Port 255 locked", TerminalStyle.DANGER));
    }

    string folderColor = "#ae1dd6";
    string fileColor = "#d61dc6";
    static string separatorColor = "#ffb7f8";
    string separator = TerminalUI.ColorText("<s>   </s>", separatorColor);

    public void PrintDirectory(FSDirectory directory) //c/user/documents
    {
        string message = directory.Name;
        string preUnused = FileSystem.GetDirectoryPath(directory);
        int fileCount = directory.Files.Count;
        int dirCount = directory.Directories.Count;
        int totalCount = fileCount + dirCount;
        int totalFileCount = GetFilesInDirectory(directory).Count;
        int parentCount = FileSystem.GetParentCount(directory);

        if (totalFileCount > 0)
        {
            foreach (FSFile file in directory.Files)
            {
                string line = "";
                for (int i = 0; i < parentCount; i++)
                {
                    line += separator;
                }
                line += TerminalUI.ColorText(file.Name, fileColor);
                TerminalCore.AddMessage(line);
            }
            foreach (FSDirectory dir in directory.Directories)
            {
                if (GetFilesInDirectory(dir).Count > 0)
                {
                    string line = "";
                    for (int i = 0; i < parentCount; i++)
                    {
                        line += separator;
                    }
                    line += TerminalUI.ColorText(dir.Name, folderColor);
                    TerminalCore.AddMessage(line);
                    PrintDirectory(dir);
                }
            }

        }
        /*
        0 -
        1 file - samel line
        1 folder - same line 
        file + folder - list
         */
        return;
    }


    public void ChangeDirectory(string[] args)
    {
        string reason;
        if (!TerminalNetwork.GetCurrentDevice().networkNode.HasAccess(out reason))
        {
            TerminalCore.AddMessage(TerminalColourScheme.FormatText(reason, TerminalStyle.DANGER));
            return;
        }


        FileSystem deviceFileSystem = TerminalNetwork.GetCurrentDevice().GetComponent<FileSystem>();
        if (deviceFileSystem != null && !deviceFileSystem.HasPermission())
        {
            NotifyBlocked();
            return;
        }

        TerminalCore.AddMessage("Changing Directory: \t" + args[0]);

        FSDirectory current = deviceFileSystem.CurrentDirectory;


        if (args[0] == "..")
        {
            if (deviceFileSystem.CurrentDirectory.Parent == null)
                return;

            deviceFileSystem.CurrentDirectory = deviceFileSystem.CurrentDirectory.Parent;
            TerminalCore.AddMessage("\tCD " + deviceFileSystem.CurrentDirectory.Name);
            return;
        }

        string[] directoryPath = args[0].Split('/');

        if (directoryPath.Length == 0)
            directoryPath = new string[] { args[0] };

        bool found = false;
        for (int i = 0; i < directoryPath.Length; i++)
        {
            bool f = false;

            directoryPath[i] = directoryPath[i].Replace("/", "");
            foreach (FSDirectory dir in current.Directories)
            {
                if (dir.Name.ToUpper() == directoryPath[i].ToUpper())
                {
                    current = dir;
                    if (i == directoryPath.Length)
                        found = true;

                    f = true;
                    break;
                }
            }

            if (!f)
                break;
        }

        deviceFileSystem.CurrentDirectory = current;

        if (!found)
            TerminalCore.AddMessage("\tCD <color=orange>" + deviceFileSystem.CurrentDirectory.Name.ToUpper() + "</color>");
        else
            TerminalCore.AddMessage("\t<color=orange>Directory not found.</color>");
    }

    public int Iterations = 0;

    public List<FSFile> GetFilesInDirectory(FSDirectory directory)
    {
        Iterations++;
        if (Iterations > 100)
            return null;

        List<FSFile> files = new List<FSFile>();
        files.AddRange(directory.Files);
        for (int i = 0; i < directory.Directories.Count; i++)
        {
            files.AddRange(GetFilesInDirectory(directory.Directories[i]));
        }

        return files;
    }

    public void PrintCommand(string[] args)
    {
        string reason;
        if (!TerminalNetwork.GetCurrentDevice().networkNode.HasAccess(out reason))
        {
            TerminalCore.AddMessage(TerminalColourScheme.FormatText(reason, TerminalStyle.DANGER));
            return;
        }

        var fs = TerminalNetwork.GetCurrentDevice().GetComponent<FileSystem>();
        if (fs != null && !fs.HasPermission())
        {
            NotifyBlocked();
            return;
        }

        string fileName = "";
        for (int i = 0; i < args.Length; i++)
        {
            fileName += args[i];
            if (i != args.Length - 1)
                fileName += " ";
        }

        FSFile file = FindFile(fileName);
        if (file != null)
        {
            string fileType = file.fileType.ToString();
            TerminalCore.AddMessage("File:" + "\t" + file.Name + ": \n\tType: " + fileType + "\n\t\"" + file.Data + "\"");
        }

    }

    public FSFile FindFile(string fileName)
    {
        string[] directoryPath = fileName.Split('/');
        var fs = TerminalNetwork.GetCurrentDevice().GetComponent<FileSystem>();
        FSDirectory current = fs.CurrentDirectory;

        if (directoryPath.Length == 0)
            directoryPath = new string[] { fileName };

        bool found = false;
        for (int i = 0; i < directoryPath.Length; i++)
        {
            if (i == directoryPath.Length - 1)
            {
                if (current.Files != null && current.Files.Count > 0)
                {
                    FSFile file = current.Files.Single(a => a.Name.ToUpper() == directoryPath[i].ToUpper());
                    return file;
                }
            }
            else
            {
                bool f = false;

                directoryPath[i] = directoryPath[i].Replace("/", "");
                foreach (FSDirectory dir in current.Directories)
                {
                    if (dir.Name.ToUpper() == directoryPath[i].ToUpper())
                    {
                        current = dir;
                        if (i == directoryPath.Length)
                            found = true;

                        f = true;
                        break;
                    }
                }

                if (!f)
                    break;
            }
        }

        return null;
    }

}
