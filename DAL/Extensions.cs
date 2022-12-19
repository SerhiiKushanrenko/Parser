using DAL.Repositories;
using DAL.Repositories.Interfaces;
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

            services.AddScoped(typeof(IFieldOfResearchRepository), typeof(FieldOfResearchRepository));

            services.AddScoped(typeof(IScientistFieldOfResearchRepository), typeof(ScientistFieldOfResearchRepository));

            services.AddScoped(typeof(IScientistRepository), typeof(ScientistRepository));

            services.AddScoped(typeof(IScientistSocialNetworkRepository), typeof(ScientistSocialNetworkRepository));

            services.AddScoped(typeof(IScientistWorkRepository), typeof(ScientistWorkRepository));

            services.AddScoped(typeof(IWorkRepository), typeof(WorkRepository));

            services.AddScoped(typeof(IOrganizationRepository), typeof(OrganizationRepository));

            services.AddScoped(typeof(IConceptRepository), typeof(ConceptRepository));
        }
    }
}
