using BLL.Interfaces;
using BLL.Parsers;
using BLL.Parsers.Interfaces;
using BLL.Servises;
using BLL.Servises.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser.Services;

namespace BLL
{
    public static class Extensions
    {
        public static void AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataLayer(configuration);

            services.AddScoped<IMainParser, MainParser>();
            services.AddScoped<ISupportParser, SupportParser>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IParserDimensions, ParserDimensions>();
            services.AddScoped<ISocialNetworkService, SocialNetworkService>();
            services.AddScoped<IWebDriver, ChromeDriver>();
        }


    }
}
