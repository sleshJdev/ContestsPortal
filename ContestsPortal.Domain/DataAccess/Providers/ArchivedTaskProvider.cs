using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using ContestsPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers
{
    public class ArchivedTaskProvider : IArchivedTaskProvider
    {
        #region Fields

        private readonly Func<PortalContext> _getContext;

        #endregion

        #region Constructors

        public ArchivedTaskProvider()
        {
        }

        public ArchivedTaskProvider(Func<PortalContext> context)
        {
            if (context == null) throw new ArgumentNullException("context");
                _getContext = context;
        }

        #endregion

        public Task<IList<ArchivedTask>> GetAllArchivedTasksAsync()
        {
            return Task<IList<ArchivedTask>>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    IList<ArchivedTask> tasks = context.ArchivedTasks.ToList();

                    return tasks;
                }
            });
        }


        public Task<ArchivedTask> GetArchivedTaskAsync(int taskId)
        {
            return Task<ArchivedTask>.Factory.StartNew(() =>
            {
                using (PortalContext context = _getContext())
                {
                    ArchivedTask task = context.ArchivedTasks.Where(a => a.TaskId.Equals(taskId)).FirstOrDefault();

                    return task;
                }
            });
        }
    }
}
