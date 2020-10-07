using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Configs
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.Property(resource => resource.MonitoringActivated).HasDefaultValue(false);
            builder.Property(resource => resource.Url).HasMaxLength(1000).IsRequired();
            builder.HasIndex(resource => resource.UserId);
            
            //builder.HasOne(resource => resource.CommunicationChanel)
            //    .WithOne()
            //    .HasForeignKey<MonitorHistory>(b => b.ResourceId)
            //    .OnDelete(DeleteBehavior.Cascade);
            
            //builder.HasOne(resource => resource.MonitorItem)
            //    .WithOne()
            //    .HasForeignKey<MonitorHistory>(b => b.ResourceId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}