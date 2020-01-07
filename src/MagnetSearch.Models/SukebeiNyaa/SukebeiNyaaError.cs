using System.Text.Json.Serialization;

namespace MagnetSearch.Models.SukebeiNyaa
{
    public class SukebeiNyaaError
    {
        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }
    }
}
