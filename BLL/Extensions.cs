using BLL.Parsers;
using BLL.Parsers.Interfaces;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BLL
{
    public static class Extensions
    {
        public static void AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataLayer(configuration);

            services.AddScoped<IScientistService, ScientistService>();
            services.AddScoped<IParsingHandler, ParcingHandler>();
            services.AddScoped<IDimensionsParser, DimensionsParser>();
            services.AddScoped<INbuviapParser, NbuviapParser>();

            services.AddScoped<IWebDriver, ChromeDriver>();
        }
    }
}
