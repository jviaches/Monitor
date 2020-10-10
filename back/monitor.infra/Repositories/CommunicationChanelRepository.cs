using monitor_infra.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monitor_infra.Repositories
{
    public class CommunicationChanelRepository : ICommunicationChanelRepository
    {
        private AppDbContext _dbContext;

        public CommunicationChanelRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool GetNotifiedByEmail(Guid id)
        {
            var result = _dbContext.CommunicationChanels.SingleOrDefault(rs => rs.Id == id);
            return result.NotifyByEmail;
        }

        public bool GetNotifiedBySlack(Guid id)
        {
            var result = _dbContext.CommunicationChanels.SingleOrDefault(rs => rs.Id == id);
            return result.NotifyBySlack;
        }

        public string GetSlackChanel(Guid id)
        {
            var result = _dbContext.CommunicationChanels.SingleOrDefault(rs => rs.Id == id);
            return result.SlackChanel;
        }
    }
}
