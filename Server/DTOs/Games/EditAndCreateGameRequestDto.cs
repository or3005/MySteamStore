using System.ComponentModel.DataAnnotations;

namespace Server.DTOs
{
    public record EditAndCreateGameRequestDto
    {
        [Required]
        [MaxLength(200)]
        public required string Title { get; init; }

        [MaxLength(50)]
        public string? Price { get; init; }

        [MaxLength(4000)]
        public string? Description { get; init; }

        [Required]
        [Url]
        public required string ImageURL { get; init; }

        public List<string>? Screenshots { get; init; }

        public int? SteamAppId { get; init; }

        public List<string>? Genre { get; init; }

        public List<string>? Developers { get; init; }
    }
}
