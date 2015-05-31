namespace ContestsPortal.Domain.DataAccess.Providers.Interfaces
{
    public interface IProviderFactory
    {
        IContestsProvider CreateIContestProvider();
    }
}
