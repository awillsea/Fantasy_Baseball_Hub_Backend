using Fantasy_Baseball_Hub_Backend.Models.Logic;
using Fantasy_Baseball_Hub_Backend.Models;
using MySql.Data.MySqlClient;

var policyName = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        builder =>
        {
            builder
            .AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
      
        });
});

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

app.UseCors(policyName);

app.UseAuthorization();

app.MapControllers();

string connstring = app.Configuration.GetConnectionString("db");
Player.DB = new MySqlConnection(connstring);
Player.ScrapePlayer();
//StandardHitterStats.DB = new MySqlConnection(connstring);
//StandardHitterStats.ScrapeSHS();
//StandardPitcherStats.DB= new MySqlConnection(connstring);
//StandardPitcherStats.ScrapeSPS();

//Webscraper.ScrapeBase();


app.Run();
