using System.ComponentModel.DataAnnotations;
using ContestsPortal.WebSite.Infrastructure.DataAttributes;

namespace ContestsPortal.WebSite.ViewModels.Account
{
    public class LoginViewModel
    {
        [RequiredIfNull("Email", ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "UserNameIsRequired")]
        [Display(ResourceType = typeof (CommonResources), Name = "Username")]
        public string UserName { get; set; }

        [EmailFormat(ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "EmailHasIncorrectFormat")]
        [RequiredIfNull("UserName", ErrorMessageResourceType = typeof (ValidationResources),
            ErrorMessageResourceName = "EmailIsRequired")]
        [Display(ResourceType = typeof (CommonResources), Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "PasswordIsRequired")]
        [Display(ResourceType = typeof (CommonResources), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof (CommonResources), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}