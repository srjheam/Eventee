using Eventee.Api.Identity.Models;

namespace Eventee.Api.Identity.Data
{
    public static class IdentityDbInitializer
    {
        public static void Initialize(ApplicationIdentityDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var users = new ApplicationIdentityUser[]
            {
                // todo
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
