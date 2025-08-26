using LibBundledGGPK3;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;

namespace PoeFixer;

public partial class MainWindow : Window
{
    public string? GGPKPath { get; set; }

    public MainWindow()
    {
        InitializeComponent();
    }

    public void EmitToConsole(string line)
    {
        if (!Dispatcher.CheckAccess())
        {
            // Если вызываем не с UI-потока — перенаправляем на Dispatcher
            Dispatcher.Invoke(() => EmitToConsole(line));
            return;
        }

        Console.Text += line + "\n";
        Console.ScrollToEnd();
    }


    //public void EmitToConsole(string line)
    //{
    //    Console.Text += line + "\n";
    //    Console.ScrollToEnd();
    //}

    private void RestoreExtractedAssets(object sender, RoutedEventArgs e)
    {
        if (GGPKPath == null)
        {
            EmitToConsole("GGPK is not selected.");
            return;
        }

        // Check if ggpk extension is .ggpk.
        try
        {
            if (GGPKPath.EndsWith(".ggpk"))
            {
                BundledGGPK ggpk = new(GGPKPath, false);
                PatchManager manager = new(ggpk.Index, this);
                int count = manager.RestoreExtractedAssets();
                ggpk.Dispose();
                EmitToConsole($"{count} assets restored.");
            }
        }
        catch (Exception ex)
        {
            EmitToConsole($"Error Restore: {ex.Message}");
        }

        if (GGPKPath.EndsWith(".bin"))
        {
            LibBundle3.Index index = new(GGPKPath, false);
            PatchManager manager = new(index, this);
            int count = manager.RestoreExtractedAssets();
            index.Dispose();
            EmitToConsole($"{count} assets restored.");
        }
    }

    private void ExtractVanillaAssets(object sender, RoutedEventArgs e)
    {
        if (GGPKPath == null)
        {
            EmitToConsole("GGPK is not selected.");
            return;
        }

        try
        {
            if (GGPKPath.EndsWith(".ggpk"))
            {
                FileExtractor extractor = new(this, GGPKPath);
                int count = extractor.ExtractFiles();
                EmitToConsole($"{count} assets extracted.");
            }
        }
        catch (Exception ex)
        {
            EmitToConsole($"Error Extract: {ex.Message}");
        }

        if (GGPKPath.EndsWith(".bin"))
        {
            FileExtractor extractor = new(this, GGPKPath);
            int count = extractor.ExtractFiles();
            EmitToConsole($"{count} assets extracted.");
        }
    }

    private void SelectGGPK(object sender, RoutedEventArgs e)
    {
        // Open file dialogue to select either a .ggpk or .bin file.
        OpenFileDialog dlg = new()
        {
            DefaultExt = ".ggpk",
            Filter = "GGPK Files (*.ggpk, *.bin)|*.ggpk;*.bin"
        };

        if (dlg.ShowDialog() == true)
        {
            GGPKPath = dlg.FileName;
            EmitToConsole($"GGPK selected: {GGPKPath}.");
        }
    }

    private void PatchGGPK(object sender, RoutedEventArgs e)
    {
        if (GGPKPath == null)
        {
            EmitToConsole("GGPK is not selected.");
            return;
        }

        EmitToConsole("Patching GGPK...");
        Stopwatch sw = new();
        sw.Start();

        try
        {
            if (GGPKPath.EndsWith(".ggpk"))
            {
                BundledGGPK ggpk = new(GGPKPath, false);
                PatchManager manager = new(ggpk.Index, this);
                int count = manager.Patch();
                ggpk.Dispose();
                EmitToConsole($"{count} assets patched.");
            }
        }
        catch (Exception ex)
        {
            EmitToConsole($"Error Patching: {ex.Message}");
        }

        if (GGPKPath.EndsWith(".bin"))
        {
            LibBundle3.Index index = new(GGPKPath, false);
            PatchManager manager = new(index, this);
            int count = manager.Patch();
            index.Dispose();
            EmitToConsole($"{count} assets patched.");
        }

        sw.Stop();
        EmitToConsole($"GGPK patched in {(int)sw.Elapsed.TotalMilliseconds}ms.");
    }
}