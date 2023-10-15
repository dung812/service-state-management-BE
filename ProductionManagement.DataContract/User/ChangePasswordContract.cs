using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.User
{
    public class ChangePasswordContract
    {
        public string CurrentPassword { get; set; } = string.Empty;
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^\._-])(?!.*\s).{8,}$", ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character")]
        public string NewPassword { get; set; } = null!;
    }
}
