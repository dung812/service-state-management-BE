using System.Threading.Tasks;
using ProductionManagement.ServiceLayer.MailConfig;

namespace ProductionManagement.ServiceLayer.Interfaces
{
    public interface IMailSenderService
    {
        Task<bool> SendAsync(Message message);
        Task<bool> SendAccountInfoAsync(string userName, string password);
        string GetAuthenticatedUsername();
    }
}
