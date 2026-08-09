

namespace Sercer.DTOs
{
    public record GameDetailsDto(Guid Id, string title, int Price, string ImageURL,
    List<string> Screenshots, int? SteamAppId, string? Description,
     List<string> Genre, List<string> Developers);
}