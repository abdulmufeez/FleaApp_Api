using fleaApi.Data;
using Microsoft.EntityFrameworkCore;

namespace fleaApi.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services
            , IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => 
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            return services;
        }

        public static async Task<WebApplication> UseAutoMigrateAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                // var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                // var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                //seeding data 
                //await Seed.SeedAppUsers(userManager, roleManager);
                //await Seed.SeedUserProfiles(context);

                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, ex.Message);
            }
            return app;
        }
    }
}