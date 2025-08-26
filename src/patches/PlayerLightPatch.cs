using Newtonsoft.Json.Linq;

namespace PoeFixer;

public class PlayerLightPatch : IPatch
{
    public float intensity = 1;

    public string[] FilesToPatch => [];

    public string[] DirectoriesToPatch => [
        "metadata/environmentsettings"
        ];

    public string Extension => "*.env";

    public string? PatchFile(string text)
    {
        try
        {
            var jsonObject = JObject.Parse(text);

            if (jsonObject["player_light"] is JObject playerLight &&
                playerLight["intensity"] is JValue intensityValue &&
                intensityValue.Type == JTokenType.Float)
            {
                playerLight["intensity"] = intensity;
            }

            return jsonObject.ToString();
        }
        catch (Exception ex)
        {
            return text;
        }
    }

    public bool ShouldPatch(Dictionary<string, bool> bools, Dictionary<string, float> floats)
    {
        bools.TryGetValue("playerLightEnabled", out bool enabled);
        floats.TryGetValue("playerLightSlider", out intensity);
        return enabled;
    }
}