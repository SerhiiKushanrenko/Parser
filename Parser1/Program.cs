
using DAL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Parser.Interfaces;
using Parser.Servises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddScoped<IMainParser, MainParser>();
builder.Services.AddScoped<ISupportParser, SupportParser>();
builder.Services.AddScoped<IRatingServise, RatingServise>();
builder.Services.AddScoped<IWebDriver, ChromeDriver>();

//builder.Services.AddScoped(typeof(IMainParser), typeof(MainParser));

//builder.Services.AddScoped(typeof(ISupportParser), typeof(SupportParser));

//builder.Services.AddScoped(typeof(IRatingServise), typeof(RatingServise));

//builder.Services.AddScoped(typeof(IWebDriver), typeof(ChromeDriver));

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
