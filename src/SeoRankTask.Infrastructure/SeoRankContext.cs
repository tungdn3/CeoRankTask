using Microsoft.EntityFrameworkCore;
using SeoRankTask.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeoRankTask.Infrastructure;

public class SeoRankContext : DbContext
{
    public SeoRankContext(DbContextOptions<SeoRankContext> options) : base(options)
    {
    }

    public DbSet<WatchListItem> WatchListItems { get; set; }

    public DbSet<HistoricalRank> HistoricalRanks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WatchListItem>().ToTable("WatchListItem");
        modelBuilder.Entity<HistoricalRank>().ToTable("HistoricalRank");
    }
}
