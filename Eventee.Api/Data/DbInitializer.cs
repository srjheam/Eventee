using Eventee.Api.Models;

namespace Eventee.Api.Data;

public static class DbInitializer
{
    public static void Initialize(EventeeContext context)
    {
        if (context.Users.Any() || context.GetTogethers.Any())
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
