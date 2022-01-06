using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModel.Auth
{
    public class RegisterViewModel
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [Required, MaxLength(50)]
        public string UserName { get; set; }
        [Required, MaxLength(255),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,MaxLength(255),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(255), DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
