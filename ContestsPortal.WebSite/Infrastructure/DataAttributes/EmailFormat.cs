using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ContestsPortal.WebSite.Infrastructure.DataAttributes
{
    public class EmailFormat : RegularExpressionAttribute, IClientValidatable
    {
        #region Detail's implementation methods

        private static readonly String _pattern;
        private bool _invalid = true;

        #region Constructors
        static EmailFormat()
        {
            _pattern =
                @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$";
        }

        public EmailFormat()
            : base(_pattern)
        {
            
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value.GetType() != typeof(String))
                    throw new Exception("Being checked data type is not a System.String type.");

                var str = value as String;

                if (IsValidEmail(str))
                    return ValidationResult.Success;
                return new ValidationResult("Некорректный E-mail адрес.");
            }
            return ValidationResult.Success;
        }

        #endregion

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var validationRule = new ModelClientValidationRegexRule(ErrorMessage, _pattern)
            {
                ValidationType = "emaildatavalidation",
                ErrorMessage = ErrorMessage
            };
            yield return validationRule;
        }

        // got from here: http://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx

        private string DomainMapper(Match match)
        {
            var idn = new IdnMapping();
            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                _invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }


        private bool IsValidEmail(string input)
        {
            _invalid = false;
            if (String.IsNullOrEmpty(input))
                return false;

            try
            {
                input = Regex.Replace(input, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (_invalid)
                return false;

            try
            {
                return Regex.IsMatch(input,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        #endregion
        }
    }
}