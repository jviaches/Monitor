using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Services.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string input);
        string Decrypt(string input);
    }
}
