using System;
using System.Collections.Generic;
using System.Text;

namespace monitor_infra.Repositories.Interfaces
{
    public interface ICommunicationChanelRepository
    {
        bool GetNotifiedByEmail(Guid resourceId);
        bool GetNotifiedBySlack(Guid resourceId);
        string GetSlackChanel(Guid resourceId);
    }
}
