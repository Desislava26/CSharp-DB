using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext:DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Server=.;Database=FootballBetting;Integrated Security=True");
            }

           // base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(x => x.PrimaryKitColor)
                .WithMany(x => x.PrimaryKitTeams)
                .HasForeignKey(x => x.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.SecondaryKitColor)
                .WithMany(x => x.SecondaryKitTeams)
                .HasForeignKey(x => x.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasOne(x => x.HomeTeam)
                .WithMany(x => x.HomeGames)
                .HasForeignKey(x => x.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AwayTeam)
                .WithMany(x => x.AwayGames)
                .HasForeignKey(x => x.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            });


            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.PlayerId });

            });
            base.OnModelCreating(modelBuilder); 
        }
    }
}
