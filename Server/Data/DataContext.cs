
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;

namespace Server.Data
{

    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }




        public DbSet<Game> Games { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(message => message.Sender)
                .WithMany(user => user.SentMessages).
                HasForeignKey(message => message.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Receiver)
                      .WithMany(u => u.ReceivedMessages)
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(m => new { m.SenderId, m.ReceiverId, m.CreateAt });
            });
        }
    }





}
