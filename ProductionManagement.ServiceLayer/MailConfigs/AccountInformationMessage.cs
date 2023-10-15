using ProductionManagement.Models;

namespace ProductionManagement.ServiceLayer.MailConfig
{
    public class AccountInformationMessage
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public AccountInformationMessage(User user, string password)
        {
            Email = user.Email;
            Subject = "Your Account Login Details";
            Body = $"<p>Dear {user.Name},</p><br> <p>PMP is sending you the login information to our system.</p> <br> <p> Account: <b>{user.UserName}</b></p> <p> Password: <b>{password}</b></p> <br> <p>PMP login at here</p> <br> <p>Best Regards,</p><br> <p>TPAS</p>";
        }
    }
}
