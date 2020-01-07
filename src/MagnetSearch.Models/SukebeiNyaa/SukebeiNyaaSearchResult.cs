using System.Text.Json.Serialization;

namespace MagnetSearch.Models.SukebeiNyaa
{
    public class SukebeiNyaaSearchResult
    {
        [JsonPropertyName("torrents")]
        public SukebeiNyaaTorrent[] Torrents { get; set; }

        [JsonPropertyName("queryRecordCount")]
        public int QueryRecordCount { get; set; }

        [JsonPropertyName("totalRecordCount")]
        public int TotalRecordCount { get; set; }
    }
}
