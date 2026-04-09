using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{


    public class Game
    {

        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public required string Title { get; set; }

        public string? Price { get; set; }

        public string? Description { get; set; }
               public string ImageURL { get; set; } 

        public List<string>? Screenshots { get; set; } = [];
        public int? SteamAppId { get; set; }
        public List<string> Genre { get; set; } = [];
        public List<string> Developers { get; set; } = [];

    }



}