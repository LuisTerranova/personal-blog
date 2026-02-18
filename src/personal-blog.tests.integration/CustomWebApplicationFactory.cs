using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.Api.Models;
using Microsoft.AspNetCore.TestHost;


namespace personal_blog.tests.integration;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null) services.Remove(descriptor);
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            
            services.AddSingleton<IEmailSender<ApplicationUser>, MockEmailSender>();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", _ => { });
        });
    }
}