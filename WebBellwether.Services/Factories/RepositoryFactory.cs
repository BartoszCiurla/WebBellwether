using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBellwether.Repositories.Context;

namespace WebBellwether.Services.Factories
{
    public static class RepositoryFactory
    {
        private static WebBellwetherDbContext _context;
        public static WebBellwetherDbContext Context => _context ?? (_context = Initialize());

        private static UserManager<IdentityUser> _userManager;
        public static UserManager<IdentityUser> UserManager
            => _userManager ?? (_userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(Context)));

        private static WebBellwetherDbContext Initialize()
        {
            var context = new WebBellwetherDbContext();
            context.Database.Connection.Open();
            return context;
        }
    }
}
