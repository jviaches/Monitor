using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using monitor_infra.Converters;
using monitor_infra.Entities;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Configs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly IEncryptionService _encryptionService;
        public UserConfiguration(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var encryptionConverter = new EncryptionValueConverter(_encryptionService);

            builder.Property(user => user.Email).HasMaxLength(30).IsRequired();
            builder.Property(user => user.Password).HasMaxLength(160).IsRequired().HasConversion(encryptionConverter);
            builder.HasIndex(user => user.Email).IsUnique();
        }
    }
}
