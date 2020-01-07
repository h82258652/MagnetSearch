using System.Text.Json.Serialization;

namespace MagnetSearch.Models.SukebeiNyaa
{
    public class SukebeiNyaaFile
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("filesize")]
        public long FileSize { get; set; }
    }
}
