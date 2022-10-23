using DAL.EF;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class Extensions
    {
        public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ParserDbContext>(options => options.UseNpgsql(configuration.GetSection("ConnectionString:ParserDB").Value));

            using (var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ParserDbContext>())
                {
                    context.Database.Migrate();
                }
            }

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //services.AddScoped(typeof(IDirectionRepository), typeof(DirectionRepository));
        }
    }
}
