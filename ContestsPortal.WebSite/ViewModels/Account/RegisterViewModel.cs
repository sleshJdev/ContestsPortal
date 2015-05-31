using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ContestsPortal.WebSite.Infrastructure.DataAttributes;
using Microsoft.Owin;

namespace ContestsPortal.WebSite.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "UserNameIsRequired")]
        [Display(ResourceType = typeof(CommonResources),Name = "Username")]
        [Remote("VerificateUserName","Verification",HttpMethod = "post",ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "UniqueUserName")]
        public string UserName { get; set; }


        [Remote("VerificateUserEmail", "Verification", HttpMethod = "POST", ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "UniqueEmail")]
        [EmailFormat(ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "EmailHasIncorrectFormat")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "EmailIsRequired")]
        [Display(ResourceType = typeof(CommonResources), Name = "Email")]
        public string Email { get; set; }

        [Remote("VerificateUserNickName", "Verification",HttpMethod = "POST",ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "UniqueNickName")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "NickNameIsRequired")]
        [Display(ResourceType = typeof(CommonResources), Name = "Nickname")]
        public string NickName { get; set; }

         [PhoneNumber(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "IncorrectPhoneNumber")]
         [Display(ResourceType = typeof(CommonResources), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

         [Display(ResourceType = typeof(CommonResources), Name = "FirstName")]
        public string FirstName { get; set; }

         [Display(ResourceType = typeof(CommonResources), Name = "LastName")]
        public string LastName { get; set; }

         [Display(ResourceType = typeof(CommonResources), Name = "MiddleName")]
        public string MiddleName { get; set; }
        
         [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "BirhtdateIsRequired")]
         [Display(ResourceType = typeof(CommonResources), Name = "Birthdate")]
        public DateTime? BirthDate { get; set; }

         [Required(ErrorMessageResourceType= typeof(ValidationResources), ErrorMessageResourceName = "CountryIsRequired")]
         [Display(ResourceType = typeof(CommonResources), Name = "Country")] 
        public int CountryId { get; set; }

         [Display(ResourceType = typeof(CommonResources), Name = "ContestNotificationsEnabled")]
        public bool ContestNotificationsEnabled { get; set; }

         [Display(ResourceType = typeof(CommonResources), Name = "HomePage")]
        public string HomePage { get; set; }
         
         [DataType(DataType.MultilineText)]
         [Display(ResourceType = typeof(CommonResources), Name = "AboutYourself")]
        public string AboutYourself { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "PasswordIsRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(CommonResources), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(CommonResources), Name = "PasswordConfirmation")]
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessageResourceType = typeof(ValidationResources), 
            ErrorMessageResourceName = "PasswordConfDoesntFit")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }

        [HiddenInput]
        public string Recaptcha { get; set; }
    }
}