using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ContestsPortal.Domain.Models;

namespace ContestsPortal.WebSite.ViewModels.Administrator
{
    public class ContestEditorViewModel
    {
        #region Properties

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources),Name ="TasksCount")]
        [Range(1, 10, ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "TasksCountRange")]
        [Required(ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "TasksCountRequired")]
        public int? TasksCount { get; set; }

        [Display(ResourceType = typeof(CommonResources), Name = "ContestTitle")]
        [Required(ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "ContestTitleRequired")]
        [MaxLength(300, ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "ContestTitleLength")]
        public string ContestTitle { get; set; }

        [Display(ResourceType = typeof(CommonResources), Name = "ContestPriorityName")]
        public int ContestPriorityId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(CommonResources), Name = "ContestCommentTitle")]
        [MaxLength(300,ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "ContestCommentMaxLength")]
        public string ContestComment { get; set; }

        [Display(ResourceType = typeof(CommonResources), Name = "ContestBeginning")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "ContestBeginningRequired")]
        public DateTime? ContestBeginning { get; set; }

        public List<TaskEditorViewModel> TaskEditors { get; set; }

        #endregion
    }
}