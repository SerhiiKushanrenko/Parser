
using DAL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser.Interfaces;
using Parser.Servises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddTransient<IMainParser, MainParser>();
builder.Services.AddTransient<ISupportParser, SupportParser>();
builder.Services.AddTransient<IRatingServise, RatingServise>();
builder.Services.AddScoped<IWebDriver, ChromeDriver>();

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
