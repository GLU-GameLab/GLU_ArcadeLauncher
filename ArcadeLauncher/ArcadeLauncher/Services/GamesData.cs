using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArcadeLauncher.Models;
using Microsoft.EntityFrameworkCore;

namespace ArcadeLauncher.Services
{
    public class GamesData : DbContext
    {
        public DbSet<GameInfo> GameInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=GLU_Arcade;Username=Arcade;Password=GLU_Arcade");
    }

}
