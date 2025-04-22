using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace PoeFixer;

public class FileExtractor
{
    private readonly MainWindow _mainWindow;
    private string? _GGPKPath;

    public const string extractJsonPath = "paths_to_extract.json";

    public FileExtractor(MainWindow mainWindow, string? gGPKPath)
    {
        _mainWindow = mainWindow;
        _GGPKPath = gGPKPath;
    }

    /// <summary>
    /// Extracts vanilla assets.
    /// </summary>
    /// <returns>Number of files extracted.</returns>
    public int ExtractFiles()
    {
        string cachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extractedassets");
        string extractBundledGGPK3Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LibGGPK3", "ExtractBundledGGPK3.exe");

        if (Directory.Exists(cachePath))
        {
            _mainWindow.EmitToConsole("Directory \"extractedassets\" exists, deleting");
            Directory.Delete(cachePath, true);
        }

        if (!File.Exists(extractBundledGGPK3Path))
        {
            _mainWindow.EmitToConsole("The file ExtractBundledGGPK3.exe was not found!!! " +
                "Download https://github.com/aianlinb/LibGGPK3 and put the downloaded files into the " +
                "LibGGPK3 folder in the root directory of the program.");

            return 0;
        }

        if (_GGPKPath == null)
        {
            _mainWindow.EmitToConsole("GGPK is not selected.");
            return 0;
        }

        if (IsFileLocked(_GGPKPath))
        {
            _mainWindow.EmitToConsole("The file is occupied by another process.");
            return 0;
        }

        int count = 0;

        PathData pathData = JsonConvert.DeserializeObject<PathData>(File.ReadAllText(extractJsonPath))!;

        foreach (var path in pathData.paths)
        {
            try
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = extractBundledGGPK3Path,
                    Arguments = $"\"{_GGPKPath}\" \"{path}\" \"{cachePath}\"",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                _mainWindow.EmitToConsole($"Start: {extractBundledGGPK3Path + startInfo.Arguments}");
                using Process process = new();
                process.StartInfo = startInfo;

                process.Start();
                process.WaitForExit();

                _mainWindow.EmitToConsole($"Extraction completed {process.ExitCode}.");
            }
            catch (Exception ex)
            {
                _mainWindow.EmitToConsole("Error ExtractBundledGGPK3.exe: " + ex.Message);
                continue;
            }
            count++;
        }

        //LibGGPK3 ERROR I'm too lazy to remake it, I'll just extract it by hand
        //foreach (string path in pathData.paths)
        //{
        //    index.TryFindNode(path, out ITreeNode? node);
        //    if (node == null) continue;

        //    string directory = Path.GetDirectoryName(path)!;

        //    count += LibBundle3.Index.ExtractParallel(node, $"{cachePath}{directory}");
        //}

        return count;
    }

    public static bool IsFileLocked(string filePath)
    {
        try
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                return false;
            }
        }
        catch (IOException)
        {
            return true;
        }
    }
}