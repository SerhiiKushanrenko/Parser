
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Parser1.EF;
using Parser1.Interfaces;
using Parser1.Servises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("BloggingDatabase")));

builder.Services.AddTransient<IMainParser, MainParser>();
builder.Services.AddTransient<ISupportParser, SupportParser>();
builder.Services.AddTransient<IRatingServise, RatingServise>();

builder.Services.AddControllers();
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, x) =>
    {
        x.Host(new Uri(builder.Configuration["RabbitMQ:Uri"]));
    });
});
//builder.Services.AddQuartzHostedService(cfg => cfg.WaitForJobsToComplete = true);

//builder.Services.AddQuartzJobs(builder.Configuration);


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
