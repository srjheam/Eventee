using Eventee.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Eventee.Api.Identity.Data;
using Eventee.Api.Identity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Eventee.Api.Factories;
using Eventee.Api.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<EventeeContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("EventeeContext")));

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationIdentityDbContext")));

builder.Services.AddIdentity<ApplicationIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddSingleton<IPagedResponseFactory>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor!.HttpContext!.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new PagedResponseFactory(new Uri(uri));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var eventeeContext = services.GetRequiredService<EventeeContext>();
    eventeeContext.Database.EnsureCreated();
    EventeeDbInitializer.Initialize(eventeeContext);

    var identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
    identityDbContext.Database.EnsureCreated();
    IdentityDbInitializer.Initialize(identityDbContext);
}

app.UseHttpsRedirection();

app.UseAuthentication();;

app.UseAuthorization();

app.MapControllers();

app.Run();
