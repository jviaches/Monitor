using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Configs
{
   public class MonitorItemConfiguration : IEntityTypeConfiguration<MonitorItem>
    {
        public void Configure(EntityTypeBuilder<MonitorItem> builder)
        {
            builder.HasMany(resource => resource.History);
        }
    }
}