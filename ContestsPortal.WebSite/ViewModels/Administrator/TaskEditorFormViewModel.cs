using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ContestsPortal.WebSite.ViewModels.Administrator
{
    public class TaskEditorFormViewModel
    {
        #region Properties

        [HiddenInput]
        public int TaskId { get; set; }

        public TimeSpan? TaskDuration { get; set; }

        public string TaskTitle { get; set; }

        public string TaskContent { get; set; }

        public string TaskComment { get; set; }

        public int TaskAward { get; set; }

        public int IdTaskCategory { get; set; }

        #endregion
    }
}