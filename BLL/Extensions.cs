

using BLL.Interfaces;
using BLL.Servises;
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
            services.AddTransient<IRatingServise, RatingServise>();
            services.AddScoped<IWebDriver, ChromeDriver>();
        }


    }
}
