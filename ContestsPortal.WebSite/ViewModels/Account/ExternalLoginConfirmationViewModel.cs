using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ContestsPortal.WebSite.Infrastructure.DataAttributes;

namespace ContestsPortal.WebSite.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        #region Properties

        [Remote("VerificateUserEmail", "Verification", HttpMethod = "POST", ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "UniqueEmail")]
        [EmailFormat(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "EmailHasIncorrectFormat")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "EmailIsRequired")]
        [Display(ResourceType = typeof(CommonResources), Name = "Email")]
        public string Email { get; set; }

        [Remote("VerificateUserNickName", "Verification", HttpMethod = "POST", ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "UniqueNickName")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "NickNameIsRequired")]
        [Display(ResourceType = typeof(CommonResources), Name = "Nickname")]
        public string NickName { get; set; }

        #endregion

    }
}