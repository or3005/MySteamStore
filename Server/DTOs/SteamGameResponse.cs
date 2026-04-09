using Server.Data;
using System.Text.Json.Serialization;
using Server.Models;
using Server.Services;



namespace Server.DTOs
{


    public class SteamGameData
    {
        [JsonPropertyName("name")]
        public string Title { get; set; }

        [JsonPropertyName("detailed_description")]
        public string Description { get; set; }

        [JsonPropertyName("header_image")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("genres")]
        public List<SteamGenre> Genres { get; set; }

        [JsonPropertyName("price_overview")]
        public SteamPrice Price { get; set; }

        [JsonPropertyName("developers")]
        public List<string> Developers { get; set; }

        [JsonPropertyName("screenshots")]
        public List<SteamScreenshot> Screenshots { get; set; }

    }
    public class SteamScreenshot
    {
        [JsonPropertyName("path_full")]
        public string PathFull { get; set; }
    }

    public class SteamPrice
    {
        [JsonPropertyName("final_formatted")]
        public string FinalPrice { get; set; }
    }

    public class SteamGenre
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class SteamAppDetailsResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public SteamGameData Data { get; set; }
    }
}