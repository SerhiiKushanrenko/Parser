

using BLL.Interfaces;
using BLL.Parsers;
using BLL.Servises;
using BLL.Servises.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser.Servises;

namespace BLL
{
    public static class Extensions
    {
        public static void AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataLayer(configuration);

            services.AddTransient<IMainParser, MainParser>();
            services.AddTransient<ISupportParser, SupportParser>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<ISocialNetworkService, SocialNetworkService>();
            services.AddScoped<IWebDriver, ChromeDriver>();
        }


    }
}
