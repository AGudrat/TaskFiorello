using System.ComponentModel.DataAnnotations;

namespace Fiorello.Models
{
    public class ChangePassword
    {
        [Required, MaxLength(255), DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required, MaxLength(255), DataType(DataType.Password)]
        public string NewPassword { get; set;}
        [Required, MaxLength(255), DataType(DataType.Password), Compare("NewPassword", ErrorMessage = "Confirm new password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
