using POC.Configuration;
using POC.Services;
using POC.Services.Publishers;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddMassTransitIntegration(builder.Configuration);
builder.Services.AddScoped<PublishDeleteWhat>();
builder.Services.AddScoped<PublishWhatDeleted>();
builder.Services.AddScoped<PublishSomethingCreated>();
builder.Services.AddHostedService<TimedExecuter>();
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
