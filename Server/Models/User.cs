using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class User
    {
        [Key]
        [Required]
        public Guid Id{get;set;}=Guid.NewGuid();


        public string? UserName {get;set;}

        public string? Password {get;set;}

    }


}