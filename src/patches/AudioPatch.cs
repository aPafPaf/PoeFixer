namespace PoeFixer;

public class AudioPatch : IPatch
{
    public string[] FilesToPatch => [];

    public string[] DirectoriesToPatch => [
        "metadata/environmentsettings"
        ];

    public string Extension => "*.env";

    public string? PatchFile(string text)
    {
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        int c = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (c == 3) break;

            if (lines[i].Contains("\"ambient_sound\":"))
            {
                lines[i] = "    \"ambient_sound\": \"\",";
                c++;
                continue;
            }

            if (lines[i].Contains("\"music\":"))
            {
                lines[i] = "    \"music\": \"\",";
                c++;
                continue;
            }

            if (lines[i].Contains("\"ambient_bank\":"))
            {
                lines[i] = "    \"ambient_bank\": \"\",";
                c++;
                continue;
            }
        }

        string modifiedText = string.Join(Environment.NewLine, lines);

        return modifiedText;
    }

    public bool ShouldPatch(Dictionary<string, bool> bools, Dictionary<string, float> floats)
    {
        bools.TryGetValue("removeAudio", out bool enabled);
        return enabled;
    }
}