using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.DataAccess.Providers;
using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using Ninject.Modules;
using Ninject.Web.Mvc;

namespace ContestsPortal.WebSite.Infrastructure.Di
{
    public class RepositoriesModule: MvcModule
    {
        public override void Load()
        {
            Bind<ICompetitorProvider>().To<CompetitorProvider>().InThreadScope();
            Bind<IPostProvider>().To<PostProvider>().InThreadScope();
            Bind<IArchivedTaskProvider>().To<ArchivedTaskProvider>().InThreadScope();
            Bind<IProgrammingLanguageProvider>().To<ProgrammingLanguageProvider>().InThreadScope();
            Bind<IUsersProvider>().To<UsersProvider>().InThreadScope();
            Bind<IContestsProvider>().To<ContestProvider>().InThreadScope();
            Bind<Func<PortalContext>>().ToMethod(x => (() => new PortalContext())).InThreadScope();
        }
    }


    public class InfrastructureModule: MvcModule
    {
        public override void Load()
        {
            

        }
    }
}