namespace PoeFixer;

public class AtlasFogOfWarPatch : IPatch
{
    public string[] FilesToPatch => [
        "metadata/materials/environment/worldmap/worldmap_fogofwar.fxgraph"
        ];

    public string[] DirectoriesToPatch => [];

    public string Extension => "*.fxgraph";

    public string? PatchFile(string text)
    {
        text = "{\r\n    \"version\": 6,\r\n    \"shader_group\": [\"Material\", \"Temporary\"],\r\n    \"overriden_blend_mode\": \"DownScaledAddablend\",\r\n    \"lighting_disabled\": true,\r\n    \"render_state_overwrite\": {\r\n        \"depth_write_enable\": false\r\n    },\r\n    \"nodes\": [],\r\n    \"links\": []\r\n}";

        return text;
    }

    public bool ShouldPatch(Dictionary<string, bool> bools, Dictionary<string, float> floats)
    {
        bools.TryGetValue("removeAtlasFogOfWar", out bool enabled);
        return enabled;
    }
}