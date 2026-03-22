
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;

namespace Server.Data
{

    public class DataContext:DbContext 
    {

        public DataContext(DbContextOptions<DataContext> options): base(options){}



        public DbSet<Game> Games {get;set;}






    }





}
