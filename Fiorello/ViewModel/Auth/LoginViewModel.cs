using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModel.Auth
{
    public class LoginViewModel
    {
        [Required, MaxLength(255), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MaxLength(255), DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
