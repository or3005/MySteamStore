using System.Text.Json.Serialization;

namespace Server.DTOs
{
    // הקומה העליונה ביותר - המעטפת של Steam
    public class SteamLibraryRoot
    {
        [JsonPropertyName("response")]
        public SteamLibraryResponse Response { get; set; }
    }

    // הקומה השנייה - מכילה את הרשימה עצמה
    public class SteamLibraryResponse
    {
        [JsonPropertyName("games")]
        public List<SteamOwnedGame> MyGames { get; set; }
    }

    public class SteamOwnedGame
    {
        [JsonPropertyName("appid")]
        public int AppId { get; set; } // ב-Steam זה בדרך כלל מספר (int)

        [JsonPropertyName("name")]
        public string Title { get; set; }
    }
}