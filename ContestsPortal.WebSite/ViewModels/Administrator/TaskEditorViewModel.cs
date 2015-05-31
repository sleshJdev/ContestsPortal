using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Razor.Parser;
using ContestsPortal.Domain.Models;

namespace ContestsPortal.WebSite.ViewModels.Administrator
{ 
    public class TaskEditorViewModel
    {
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskComplexity")] 
        [Required(ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "TaskComplexityRequired")]
        [Range(1,20,ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "TaskComplextityRange")]
        public int? TaskComplexity  {get; set;}
        
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskDuration")] 
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskDurationRequired")]
        [Range(typeof(TimeSpan),"00:00:00", "05:00:00", ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskDurationRange")]
        public TimeSpan? TaskDuration { get; set; }

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskAward")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskAwardRequired")]
        [Range(1,300, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskAwardRange")]
        public int TaskAward { get; set; }

        [Display(ResourceType = typeof(CommonResources), Name = "TaskTitle")] 
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskTitleRequired")]
        [MaxLength(200,ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskTitleMaxLength")]
        public string TaskTitle { get; set; }

        public IList<ProgrammingLanguageViewModel> Languages { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskContent")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskContentRequired")]
        public string TaskContent { get; set;}
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskComment")]
        [MaxLength(300, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskCommentMaxLength")]
        public string TaskComment { get; set;}
    }
}