using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using ContestsPortal.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers
{
    public class TaskProvider : ITaskProvider
    {
        #region Fields

        private readonly Func<PortalContext> _getContext;

        #endregion

        #region Constructors

        public TaskProvider()
        {
        }

        public TaskProvider(Func<PortalContext> context)
        {
            if (context == null) throw new ArgumentNullException("context");
                _getContext = context;
        }

        #endregion
 

        public Task<IList<ContestTask>> GetAllContestTasksAsync()
        {
            return Task<IList<ContestTask>>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    IList<ContestTask> tasks = context.ContestTasks.ToList();

                    return tasks;
                }
            });
        }

        private class TaskComparator : IEqualityComparer<ContestTask>
        {
            public bool Equals(ContestTask x, ContestTask y)
            {
                return x.TaskId.Equals(y.TaskId);
            }

            public int GetHashCode(ContestTask obj)
            {
                throw new NotImplementedException();
            }
        }

        public Task<ContestTask> GetContestTaskAsync(int taskId)
        {
            return Task<ContestTask>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    ContestTask task = context.ContestTasks.Include("Languages").Single(a => a.TaskId.Equals(taskId));
                    
                    return task;
                }
            });
        }
    }
}
