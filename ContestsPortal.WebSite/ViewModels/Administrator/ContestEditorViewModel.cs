using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ContestsPortal.Domain.Models;

namespace ContestsPortal.WebSite.ViewModels.Administrator
{
    public class ContestEditorViewModel
    {
        private Contest _contest;

        public ContestEditorViewModel()
        {
            _contest = new Contest();
        }

        public ContestEditorViewModel(Contest contest)
        {
            _contest = contest;
        }

        #region Properties

        public int ContestId { get { return _contest.ContestId; } set { _contest.ContestId = value; } }

        [Display(ResourceType = typeof(CommonResources), Name = "ContestEnd")]
        [DataType(DataType.DateTime)]
        public DateTime? ContestEnd { get { return _contest.ContestEnd; } set { _contest.ContestEnd = value; } }

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources), Name = "CompetitorsCount")]
        public int CompetitorsCount { get { return _contest.CompetitorsCount; } set { _contest.CompetitorsCount = value; } }

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(CommonResources),Name ="TasksCount")]
        [Range(1, 10, ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "TasksCountRange")]
        [Required(ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "TasksCountRequired")]
        public int? TasksCount { get { return _contest.TasksCount; } set { _contest.TasksCount = value??0; } }

        [Display(ResourceType = typeof(CommonResources), Name = "ContestTitle")]
        [Required(ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "ContestTitleRequired")]
        [MaxLength(300, ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "ContestTitleLength")]
        public string ContestTitle { get { return _contest.ContestTitle; } set { _contest.ContestTitle = value; } }

        [Display(ResourceType = typeof(CommonResources), Name = "ContestPriorityName")]
        public int ContestPriorityId { get { return _contest.IdContestPriority; } set { _contest.IdContestPriority = value; } }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(CommonResources), Name = "ContestCommentTitle")]
        [MaxLength(300,ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "ContestCommentMaxLength")]
        public string ContestComment { get { return _contest.ContestComment; } set { _contest.ContestComment = value; } }

        [Display(ResourceType = typeof(CommonResources), Name = "ContestBeginning")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessageResourceType = typeof (ValidationResources),ErrorMessageResourceName = "ContestBeginningRequired")]
        public DateTime? ContestBeginning { get { return _contest.ContestBeginning; } set { _contest.ContestBeginning = value; } }

        public List<TaskEditorViewModel> TaskEditors { get; set; }

        #endregion
    }
}