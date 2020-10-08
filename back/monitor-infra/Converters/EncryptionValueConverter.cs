using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using monitor_infra.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Converters
{
    public class EncryptionValueConverter: ValueConverter<string, string>
    {
        public EncryptionValueConverter(IEncryptionService encryptionService):
            base(enc => encryptionService.Encrypt(enc), enc=> encryptionService.Decrypt(enc))
        {
        }
    }
}
