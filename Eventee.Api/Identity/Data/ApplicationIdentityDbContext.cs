using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eventee.Api.Identity.Models;

namespace Eventee.Api.Identity.Data;

public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationIdentityUser>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}
