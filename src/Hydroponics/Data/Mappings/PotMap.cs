using Hydroponics.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydroponics.Data.Mappings;

internal class PotMap : IEntityTypeConfiguration<Pot>
{
    public void Configure(EntityTypeBuilder<Pot> builder)
    {
        if (builder is null)
        {
            throw new System.ArgumentNullException(nameof(builder));
        }
        _ = builder.ToTable("Pot", "dbo").HasKey(s => s.Id);
    }
}