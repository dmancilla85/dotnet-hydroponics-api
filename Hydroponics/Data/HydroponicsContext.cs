using Hydroponics.Data.Mappings;
using Hydroponics.Model;
using Microsoft.EntityFrameworkCore;

namespace Hydroponics.Data;

public class HydroponicsContext : DbContext
{
  public virtual DbSet<Substrate> Substrates { get; set; }
  public virtual DbSet<CultivationMethod> CultivationMethods { get; set; }

  public HydroponicsContext(DbContextOptions<HydroponicsContext> options) : base(options)
  {
    // do something
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    if (modelBuilder == null)
    {
      throw new ArgumentNullException(nameof(modelBuilder));
    }

    base.OnModelCreating(modelBuilder);
    _ = modelBuilder.ApplyConfiguration(new SubstrateMap());
    _ = modelBuilder.ApplyConfiguration(new CultivationMethodMap());


    // Define specific decimal type for all decimal columns
    foreach (var property in modelBuilder.Model.GetEntityTypes()
      .SelectMany(t => t.GetProperties())
      .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
    {
      property.SetColumnType("decimal(11,2)");
    }
  }
}


