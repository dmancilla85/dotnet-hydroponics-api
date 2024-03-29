﻿using Hydroponics.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydroponics.Data.Mappings;

internal class CultivationMethodMap : IEntityTypeConfiguration<CultivationMethod>
{
    public void Configure(EntityTypeBuilder<CultivationMethod> builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        _ = builder.ToTable("CultivationMethod", "dbo").HasKey(s => s.Id);
    }
}