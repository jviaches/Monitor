using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Services.Interfaces
{
    public interface ITokenService
    {
        string GetToken(User user);
        string RenewToken(string token);
        int GetUserIdFromToken(string token);
        //int GetUserCompanyIdFromToken(string token);
    }
}
