using Hydroponics.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydroponics.Data.Mappings;

internal class MeasureMap : IEntityTypeConfiguration<Measure>
{
    public void Configure(EntityTypeBuilder<Measure> builder)
    {
        if (builder is null)
        {
            throw new System.ArgumentNullException(nameof(builder));
        }
        _ = builder.ToTable("Measure", "dbo").HasKey(s => s.Id);
    }
}