namespace PoeFixer.src.patches;

public class CloudPatch : IPatch
{
    public string[] FilesToPatch => [];

    public string[] DirectoriesToPatch => [
        "metadata/environmentsettings"
        ];

    public string Extension => "*.env";

    public string? PatchFile(string text)
    {
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("\"clouds_intensity\":"))
            {
                lines[i] = "    \"clouds_intensity\": 0.0,";
                break;
            }
        }

        string modifiedText = string.Join(Environment.NewLine, lines);

        return modifiedText;
    }

    public bool ShouldPatch(Dictionary<string, bool> bools, Dictionary<string, float> floats)
    {
        bools.TryGetValue("removeCloud", out bool enabled);
        return enabled;
    }
}