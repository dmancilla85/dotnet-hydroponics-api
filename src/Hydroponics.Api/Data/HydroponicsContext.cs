using Hydroponics.Data.Entities;
using Hydroponics.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Hydroponics.Data;

/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
public class HydroponicsContext(DbContextOptions<HydroponicsContext> options) : DbContext(options)
{
    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<CultivationMethod> CultivationMethods { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Measure> Measures { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Pot> Pots { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Substrate> Substrates { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder
            .ApplyConfiguration(new SubstrateMap())
            .ApplyConfiguration(new CultivationMethodMap())
            .ApplyConfiguration(new MeasureMap())
            .ApplyConfiguration(new PotMap());

        // Define specific decimal type for all decimal columns
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(11,2)");
        }
    }
}
