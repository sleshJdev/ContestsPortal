using Microsoft.AspNet.Identity.EntityFramework;

namespace ContestsPortal.Domain.DataAccess
{
    public class CustomIdentityRole : IdentityRole<int, CustomIdentityUserRole> { }

    public class CustomIdentityUserLogin : IdentityUserLogin<int> { }

    public class CustomIdentityUserRole : IdentityUserRole<int> { }

    public class CustomIdentityUserClaim : IdentityUserClaim<int> { }

}
