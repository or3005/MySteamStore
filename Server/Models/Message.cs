using System.ComponentModel.DataAnnotations;
using Server.Models;
namespace Server.Models
{




    public class Message
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Content { get; set; }

        public User Receiver { get; set; } = null!;
        public Guid ReceiverId { get; set; }

        public Guid SenderId { get; set; }

        public User Sender { get; set; } = null!;

        public DateTime CreateAt { get; set; }

    }






}