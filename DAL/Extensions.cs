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
            services.AddDbContext<ParserDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ParserDB")));

            using (var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ParserDbContext>())
                {
                    context.Database.Migrate();
                }
            }

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped(typeof(IFieldOfResearchRepository), typeof(FieldOfResearchRepository));

            services.AddScoped(typeof(IScientistFieldOfResearchRepository), typeof(ScientistFieldOfResearchRepository));

            services.AddScoped(typeof(IScientistRepository), typeof(ScientistRepository));

            services.AddScoped(typeof(IScientistSocialNetworkRepository), typeof(ScientistSocialNetworkRepository));

            services.AddScoped(typeof(IScientistWorkRepository), typeof(ScientistWorkRepository));

            services.AddScoped(typeof(IWorkRepository), typeof(WorkRepository));


            //services.AddScoped<IRepository<Scientist>, ScientistRepository>();
            //services.AddScoped<IRepository<FieldOfResearch>, FieldOfResearchRepository>();
            //services.AddScoped<IRepository<ScientistFieldOfResearch>, ScientistFieldOfResearchRepository>();
            //services.AddScoped<IRepository<ScientistSocialNetwork>, ScientistSocialNetworkRepository>();
            //services.AddScoped<IRepository<ScientistWork>, ScientistWorkRepository>();
            //services.AddScoped<IRepository<Work>, WorkRepository>();

            //services.AddScoped<IFieldOfResearchRepository, FieldOfResearchRepository>();
            //services.AddScoped<IScientistFieldOfResearchRepository, ScientistFieldOfResearchRepository>();
            //services.AddScoped<IScientistRepository, ScientistRepository>();
            //services.AddScoped<IScientistSocialNetworkRepository, ScientistSocialNetworkRepository>();
            //services.AddScoped<IScientistWorkRepository, ScientistWorkRepository>();
            //services.AddScoped<IWorkRepository, WorkRepository>();

            //services.AddScoped(typeof(IDirectionRepository), typeof(DirectionRepository));
        }
    }
}
