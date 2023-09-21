using Eventee.Api.Data;
using Eventee.Api.Models;

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

            var users = new User[]
            {
                // todo
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var getTogethers = new GetTogether[]
            {
                // todo
            };

            context.GetTogethers.AddRange(getTogethers);
            context.SaveChanges();
        }
    }
}
