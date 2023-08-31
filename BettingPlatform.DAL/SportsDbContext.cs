using BettingPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BettingPlatform.DAL
{
    public class SportsDbContext : DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options)
        {
            
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Odd> Odds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            SportModelBuilder(builder);
            EventModelBuilder(builder);
            BetModelBuilder(builder);
            MatchModelBuilder(builder);
            OddModelBuilder(builder);

            base.OnModelCreating(builder);
        }

        private static void OddModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Odd>().HasKey(o => o.ID);
        }

        private static void MatchModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Match>().HasKey(m => m.ID);
            builder.Entity<Match>().HasMany(m => m.Bet);
        }

        private static void BetModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Bet>().HasKey(b => b.ID);
            builder.Entity<Bet>().HasMany(b => b.Odd);
        }

        private static void EventModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Event>().HasKey(e => e.ID);
            builder.Entity<Event>().HasMany(e => e.Match);
        }

        private static void SportModelBuilder(ModelBuilder builder)
        {
            builder.Entity<Sport>().HasKey(s => s.ID);
            builder.Entity<Sport>().HasMany(s => s.Event);
        }
    }
}
