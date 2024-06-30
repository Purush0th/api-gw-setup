using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGw
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                     options =>
                     {
                         options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
                     });


            services.AddIdentityApiEndpoints<AppUser>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

            services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromSeconds(10);
            });

            // Configure CORS
            services
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                               builder.SetIsOriginAllowed(_ => true)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials());
                });

            // Configure health checks
            services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();

            services.AddOcelot();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
           {
               _ = endpoints.MapControllers();
               // Map Identity
               endpoints.MapGroup("/identity").MapIdentityApi<AppUser>();

               endpoints.MapPost("/identity/logout", async (SignInManager<AppUser> signInManager, object empty) =>
                               {
                                   if (empty != null)
                                   {
                                       await signInManager.SignOutAsync();
                                       return Results.Ok();
                                   }
                                   return Results.Unauthorized();
                               })
                .WithOpenApi()
                .RequireAuthorization();

               // Map health check endpoints
               endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
               {
                   Predicate = _ => true,
                   ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
               });

               endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
               {
                   Predicate = r => r.Name.Contains("self")
               });

           });


            app.UseCors("CorsPolicy");
            app.UseOcelot().Wait();
        }
    }
}
