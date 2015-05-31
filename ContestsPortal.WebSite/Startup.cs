using Owin;

namespace ContestsPortal.WebSite
{
   public partial class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
           ConfigureAuth(builder);
          
        }
    }
}