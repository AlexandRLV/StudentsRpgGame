using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameSettings
{
    [JsonObject]
    public class SoundSettings
    {
        [JsonIgnore] public float Volume => enabled ? volume : 0f; 
        
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")] public SoundVolumeType soundType;
        
        [JsonProperty("value")] public float volume;
        [JsonProperty("enabled")] public bool enabled;
    }
}