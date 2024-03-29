﻿using Hydroponics.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydroponics.Data.Mappings;

internal class SubstrateMap : IEntityTypeConfiguration<Substrate>
{
    public void Configure(EntityTypeBuilder<Substrate> builder)
    {
        if (builder is null)
        {
            throw new System.ArgumentNullException(nameof(builder));
        }
        _ = builder.ToTable("Substrate", "dbo").HasKey(s => s.Id);
    }
}