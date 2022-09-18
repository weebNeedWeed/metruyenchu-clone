using System.ComponentModel.DataAnnotations;

namespace TheStory.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(255)]
        [MinLength(8, ErrorMessage = "Password must be between 8 and 255 characters")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password must match with Password")]
        public string? ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        public bool IsPersistent { get; set; }
    }

    public class AuthViewModel
    {
        public RegisterViewModel? RegisterViewModel { get; set; }
        public LoginViewModel? loginViewModel { get; set; }
    }
}
