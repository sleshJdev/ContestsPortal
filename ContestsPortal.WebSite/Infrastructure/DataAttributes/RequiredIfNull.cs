using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ContestsPortal.WebSite.Infrastructure.DataAttributes
{
    public class RequiredIfNull: ValidationAttribute,IClientValidatable
    {
        private readonly string _propName;

        #region Constructors

        public RequiredIfNull(string propName)
        {
            if (string.IsNullOrEmpty(propName)) throw new ArgumentNullException("propName");
            _propName = propName;
        }

        public RequiredIfNull()
        {
        }

        #endregion

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_propName);
            if (property== null)
                throw new ArgumentException("There are no such property in validation object.");

            var val = property.GetValue(validationContext.ObjectInstance) == null;

            if (val)
                return new RequiredAttribute().IsValid(value)
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var vrule = new ModelClientValidationRule
            {
                ValidationType = "requiredifnull",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            vrule.ValidationParameters.Add(new KeyValuePair<string, object>("otherprop",_propName));
            yield return vrule;
        }
    }
}