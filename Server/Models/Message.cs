using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    



    public class Message
    {
        [Key]
        [Required]
        public Guid Id{get;set;}=Guid.NewGuid();

        public string? Content {get;set;}

        public Guid? ReceiverId {get;set;}

        public Guid? SenderId {get;set;}
        
        public DateTime CreateAt {get;set;}

    }






}