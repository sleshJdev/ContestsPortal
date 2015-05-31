using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ContestsPortal.WebSite.Infrastructure
{
    public static class PasswordValidatorExtensions    {

        public static PasswordValidator ClonePasswordValidator(this PasswordValidator passwordValidator)
        {
            return new PasswordValidator()
            {
                RequireDigit = passwordValidator.RequireDigit,
                RequireLowercase = passwordValidator.RequireLowercase,
                RequireNonLetterOrDigit = passwordValidator.RequireNonLetterOrDigit,
                RequireUppercase = passwordValidator.RequireUppercase,
                RequiredLength = passwordValidator.RequiredLength
            };
        }

        public static PasswordValidator CreateNotValidatingPasswordValidator()
        {
            return new PasswordValidator()
            {
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
                RequiredLength = 0
            };
        }
    }
}