using System;
using System.Text.Json.Serialization;

namespace MagnetSearch.Models.SukebeiNyaa
{
    public class SukebeiNyaaTorrent
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("filesize")]
        public long FileSize { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("comments")]
        public object[] Comments { get; set; }

        [JsonPropertyName("sub_category")]
        public string SubCategory { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("anidbid")]
        public int Anidbid { get; set; }

        [JsonPropertyName("vndbid")]
        public int Vndbid { get; set; }

        [JsonPropertyName("vgmdbid")]
        public int Vgmdbid { get; set; }

        [JsonPropertyName("dlsite")]
        public string Dlsite { get; set; }

        [JsonPropertyName("videoquality")]
        public string Videoquality { get; set; }

        [JsonPropertyName("tags")]
        public object Tags { get; set; }

        [JsonPropertyName("uploader_id")]
        public int UploaderId { get; set; }

        [JsonPropertyName("uploader_name")]
        public string UploaderName { get; set; }

        [JsonPropertyName("uploader_old")]
        public string UploaderOld { get; set; }

        [JsonPropertyName("website_link")]
        public string WebsiteLink { get; set; }

        [JsonPropertyName("languages")]
        public string[] Languages { get; set; }

        [JsonPropertyName("magnet")]
        public string Magnet { get; set; }

        [JsonPropertyName("torrent")]
        public string Torrent { get; set; }

        [JsonPropertyName("seeders")]
        public int Seeders { get; set; }

        [JsonPropertyName("leechers")]
        public int Leechers { get; set; }

        [JsonPropertyName("completed")]
        public int Completed { get; set; }

        [JsonPropertyName("last_scrape")]
        public DateTime LastScrape { get; set; }

        [JsonPropertyName("file_list")]
        public SukebeiNyaaFile[] FileList { get; set; }
    }
}
