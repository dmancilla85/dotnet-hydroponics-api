using Hydroponics.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydroponics.Data.Mappings
{
  public class CultivationMethodMap : IEntityTypeConfiguration<CultivationMethod>
  {
    public void Configure(EntityTypeBuilder<CultivationMethod> builder)
    {
      if (builder is null)
      {
        throw new System.ArgumentNullException(nameof(builder));
      }
      _ = builder.ToTable("CultivationMethod", "dbo").HasKey(s => s.Id);
    }
  }
}
