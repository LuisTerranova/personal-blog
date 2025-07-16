using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder
    .Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(x=>
    x.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();

var app = builder.Build();
app.MapGet("/", () => new {message = "OK"});

app.Run();
