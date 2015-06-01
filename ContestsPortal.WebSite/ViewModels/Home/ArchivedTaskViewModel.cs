using ContestsPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContestsPortal.WebSite.ViewModels.Home
{
    public class ArchivedTaskViewModel
    {
        private ArchivedTask _task;

        public ArchivedTaskViewModel(ArchivedTask task)
        {
            if (task == null) throw new ArgumentNullException("task");

            _task = task;
        }
    }
}