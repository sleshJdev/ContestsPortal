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
        private ContestTask _task;

        public TaskEditorViewModel()
        {
            _task = new ContestTask();
        }

        public TaskEditorViewModel(ContestTask task)
        {
            _task = task;
        }

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskComplexity")] 
        [Required(ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "TaskComplexityRequired")]
        [Range(1,20,ErrorMessageResourceType = typeof(ValidationResources),ErrorMessageResourceName = "TaskComplextityRange")]
        public int? TaskComplexity { get { return _task.TaskComplexity; } set { _task.TaskComplexity = value ?? 0; } }
        
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskDuration")] 
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskDurationRequired")]
        [Range(typeof(TimeSpan),"00:00:00", "05:00:00", ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskDurationRange")]
        public TimeSpan? TaskDuration { get { return _task.TaskDuration; } set { _task.TaskDuration = value; } }

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskAward")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskAwardRequired")]
        [Range(1,300, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskAwardRange")]
        public int TaskAward { get { return _task.TaskAward; } set { _task.TaskAward = value; } }

        [Display(ResourceType = typeof(CommonResources), Name = "TaskTitle")] 
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskTitleRequired")]
        [MaxLength(200,ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskTitleMaxLength")]
        public string TaskTitle { get { return _task.TaskTitle; } set { _task.TaskTitle = value; } }

        public IList<ProgrammingLanguageViewModel> Languages { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskContent")]
        [Required(ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskContentRequired")]
        public string TaskContent { get { return _task.TaskContent; } set { _task.TaskContent = value; } }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(CommonResources), Name = "TaskComment")]
        [MaxLength(300, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "TaskCommentMaxLength")]
        public string TaskComment { get { return _task.TaskComment; } set { _task.TaskComment = value; } }
    }
}