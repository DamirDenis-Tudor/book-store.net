using Microsoft.EntityFrameworkCore;
using PetShop_BackEnd.Persistence.Entities;
using PetShop_BackEnd.Persistence.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/test", () =>
    {
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();


var dbContext = new DatabaseContext();

var user = new User
{
    Name = "hei",
    Email = "mama@tata.gmail.com",
    Password = "parola",
    UserType = "Smenar",
    BillDetails = null
};

if (dbContext.Database.EnsureCreated())
{
    Console.WriteLine("Successfully created.");
}

dbContext.Add(user);
dbContext.SaveChanges();

app.Run();
