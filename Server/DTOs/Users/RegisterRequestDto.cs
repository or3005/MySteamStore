
using System.ComponentModel.DataAnnotations;

namespace Server.DTOs
{
    public record RegisterRequestDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}