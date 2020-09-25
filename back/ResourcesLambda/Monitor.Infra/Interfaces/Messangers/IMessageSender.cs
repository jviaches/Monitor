using System.Threading.Tasks;

namespace Monitor.Infra.Interfaces.Messangers
{
    public interface IMessageSender
    {
        Task SendMessage(string channel, string message);
    }
}
