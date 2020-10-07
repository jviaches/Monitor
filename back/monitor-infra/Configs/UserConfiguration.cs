using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Configs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(user => user.Email).HasMaxLength(30).IsRequired();
            builder.Property(user => user.Password).HasMaxLength(100).IsRequired();
            builder.HasIndex(user => user.Email).IsUnique();
        }
    }
}
