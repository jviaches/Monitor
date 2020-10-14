using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace monitor.infra.Services.Interfaces
{
    public interface ISlackSenderService
    {
        void SendAbnormalStatus(string channel, string url, string statusCodeResult);
    }
}
