using System;
using ContestsPortal.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ContestsPortal.WebSite.ViewModels.Administrator
{
    public class ProgrammingLanguageViewModel
    {
        #region Constructors

        public ProgrammingLanguageViewModel()
        {
        }


        public ProgrammingLanguageViewModel(ProgrammingLanguage language)
            : this(language, false)
        { }

        public ProgrammingLanguageViewModel(ProgrammingLanguage language, bool isSelected)
        {
            if (language == null) throw new ArgumentNullException("language");
            LanguageId = language.LanguageId;
            LanguageName = language.LanguageName;
            IsSelected = isSelected;
        }

        #endregion

        #region Properties

        [Editable(false)]
        [HiddenInput(DisplayValue = false)]
        public int LanguageId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "LanguageNameIsRequired")]
        [Display(ResourceType = typeof(CommonResources), Name = "LanguageName")]
        public string LanguageName { get; set; }
        
        public bool IsSelected { get; set; }

        #endregion

    }
}